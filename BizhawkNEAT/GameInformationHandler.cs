using BizHawk.Client.ApiHawk;
using BizhawkNEAT.Utils;
using System;
using System.Collections.Generic;

namespace BizhawkNEAT
{
    public class GameInformationHandler
    {
        private JoypadApi Joypad { get; set; }
        private IMem Memory { get; set; }
        private MemorySaveStateApi SaveState { get; set; }

        private string SaveStateId { get; set; }

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

        public void SetSaveStateApi(MemorySaveStateApi memorySaveStateApi)
        {
            SaveState = memorySaveStateApi;
        }
        
        public int MarioX
        {
            get
            {
                return (int)(Memory.ReadByte(0x6D) * 0x100 + Memory.ReadByte(0x86));
            }
        }

        public int MarioY
        {
            get
            {
                return (int)Memory.ReadByte(0x03B8) + 16;
            }
        }

        private int ScreenXOffset
        {
            get
            {
                return (int)Memory.ReadByte(0x03AD);
            }
        }

        private int ScreenYOffset
        {
            get
            {
                return (int)Memory.ReadByte(0x03B8);
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

            var inputs = new int[13 * 13 + 1];
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

            // Bias
            inputs[index] = 1;

            return inputs;
        }

        private void PressButton(string button, bool value)
        {
            Joypad.Set(button, value, 1);
        }

        public void HandleOutput(bool[] output)
        {
            if(output.Length != Config.ButtonNames.Length)
            {
                throw new Exception("Too many inputs to the joypad");
            }

            // If LEFT and RIGHT are pressed
            if(output[4] && output[5])
            {
                output[4] = false;
                output[5] = false;
            }

            // If UP and DOWN are pressed
            if (output[2] && output[3])
            {
                output[2] = false;
                output[3] = false;
            }

            for (int i = 0; i < Config.ButtonNames.Length; i++)
            {
                PressButton(Config.ButtonNames[i], output[i]);
            }
        }

        public void ClearJoyPad()
        {
            for (int i = 0; i < Config.ButtonNames.Length; i++)
            {
                PressButton(Config.ButtonNames[i], false);
            }
        }

        public void LoadSaveState()
        {
            SaveState.LoadCoreStateFromMemory(SaveStateId);
        }

        public void SaveGameState()
        {
            SaveStateId = SaveState.SaveCoreStateToMemory();
        }
    }
}
