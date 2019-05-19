using BizHawk.Client.ApiHawk;
using System;
using System.Collections.Generic;

namespace BizhawkNEAT
{
    public class GameInformationHandler
    {
        [RequiredApi]
        private JoypadApi Joypad { get; set; }

        [RequiredApi]
        private IMem Memory { get; set; }

        public GameInformationHandler()
        {
        }

        public void SetJoypadApi(JoypadApi joypadApi)
        {
            Joypad = joypadApi;
        }

        public void SetMemoryApi(IMem memoryApi)
        {
            Memory = memoryApi;
        }

        private uint MarioX
        {
            get
            {
                return Memory.ReadByte(0x6D) * 0x100 + Memory.ReadByte(0x86);
            }
        }

        private uint MarioY
        {
            get
            {
                return Memory.ReadByte(0x03B8) + 16;
            }
        }

        private uint ScreenXOffset
        {
            get
            {
                return Memory.ReadByte(0x03AD);
            }
        }

        private uint ScreenYOffset
        {
            get
            {
                return Memory.ReadByte(0x03B8);
            }
        }

        private int GetTile(int dx, int dy)
        {
            var x = (double)(MarioX + dx + 8);
            var y = (double)(MarioY + dy - 16);
            var page = Math.Floor(x / 256) % 2;

            var subX = Math.Floor((x % 256) / 16);
            var subY = Math.Floor((y - 32) / 16);

            if (subY >= 13 || subY < 0)
            {
                return 0;
            }

            var address = 0x500 + page * 13 * 16 + subY * 16 + subX;
            if (Memory.ReadByte((long)address) != 0)
            {
                return 1;
            }

            return 0;
        }

        private List<Tuple<uint, uint>> GetEnemiesPositions()
        {
            var enemiesPositions = new List<Tuple<uint, uint>>();
            for (int i = 0; i < 5; i++)
            {
                var enemy = Memory.ReadByte(0xF + i);
                if (enemy != 0)
                {
                    var x = Memory.ReadByte(0x6E + i) * 0x100 + Memory.ReadByte(0x87 + i);
                    var y = Memory.ReadByte(0xCF + i) + 24;
                    var enemyPosition = new Tuple<uint, uint>(x, y);
                    enemiesPositions.Add(enemyPosition);
                }
            }

            return enemiesPositions;
        }

        public int[] GetNeuralNetInputs()
        {
            var enemiesPositions = GetEnemiesPositions();

            var inputs = new int[13 * 13];
            var index = 0;

            var marioX = MarioX;
            var marioY = MarioY;

            for (int y = -6; y <= 6; y++)
            {
                for (int x = -6; x <= 6; x++)
                {
                    inputs[index] = 0;

                    var tile = GetTile(x, y);

                    if (tile == 1 && marioY + y < 0x1B0)
                    {
                        inputs[index] = 1;
                    }

                    foreach (var enemy in enemiesPositions)
                    {
                        var xDistance = Math.Abs(enemy.Item1 - (marioX + x));
                        var yDistance = Math.Abs(enemy.Item2 - (marioY + y));

                        if(xDistance <= 8 && yDistance <= 8)
                        {
                            inputs[index] = -1;
                        }
                    }

                    index++;
                }
            }

            return inputs;
        }
    }
}
