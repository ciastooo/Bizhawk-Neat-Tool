using BizhawkNEAT.Neat;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace BizhawkNEAT.Utils
{
    public static class DrawingHelper
    {
        private static IDictionary<int, NodeDrawElement> _cachedNodes { get; set; }

        public static void ClearCache()
        {
            _cachedNodes = null;
        }

        private static readonly Dictionary<Color, SolidBrush> _solidBrushes = new Dictionary<Color, SolidBrush>();
        private static readonly Dictionary<Color, Pen> _pens = new Dictionary<Color, Pen>();
        private static SolidBrush GetBrush(Color color)
        {
            SolidBrush b;
            if (!_solidBrushes.TryGetValue(color, out b))
            {
                b = new SolidBrush(color);
                _solidBrushes[color] = b;
            }
            return b;
        }
        private static Pen GetPen(Color color, int width = 1)
        {
            Pen p;
            if (!_pens.TryGetValue(color, out p))
            {
                p = new Pen(color);
                _pens[color] = p;
            }
            return p;
        }

        public static Graphics DrawGenome(Graphics graphics, Genome genome)
        {
            graphics.Clear(SystemColors.Control);

            graphics.FillRectangle(GetBrush(Color.LightGray), 9, 9, 132, 132);
            graphics.DrawRectangle(GetPen(Color.Black), 9, 9, 132, 132);

            var nodesToDraw = GetNodesToDraw(genome);

            foreach (var node in nodesToDraw)
            {
                var nodeValue = genome.NodeGenes[node.Key].Value;
                Color color;
                switch (nodeValue)
                {
                    case -1:
                        color = Color.Black;
                        break;
                    case 0:
                        color = Color.LightGray;
                        break;
                    case 1:
                        color = Color.White;
                        break;
                    default:
                        color = Color.FromArgb((int)(nodeValue + 1) / 2 * 256);
                        break;
                }

                var opacity = 255;
                if (nodeValue == 0)
                {
                    opacity = 80;
                }

                graphics.FillRectangle(GetBrush(Color.FromArgb(opacity, color)), node.Value.X, node.Value.Y, 10, 10);
                graphics.DrawRectangle(GetPen(Color.FromArgb(opacity, color)), node.Value.X, node.Value.Y, 10, 10);
            }

            foreach (var connection in genome.ConnectionGenes.Values.Where(cg => cg.IsEnabled))
            {
                var previousNodeToDraw = nodesToDraw[connection.PreviousNode.Id];
                var nextNodeToDraw = nodesToDraw[connection.NextNode.Id];

                var opacity = 160;
                if (connection.PreviousNode.Value == 0)
                {
                    opacity = 32;
                }

                var color = connection.Weight > 0 ? Color.Green : Color.Red;

                graphics.DrawLine(GetPen(Color.FromArgb(opacity, color), 3), previousNodeToDraw.X + 9, previousNodeToDraw.Y + 4, nextNodeToDraw.X, nextNodeToDraw.Y + 4);
            }

            return graphics;
        }

        private static IDictionary<int, NodeDrawElement> GetNodesToDraw(Genome genome)
        {
            if (_cachedNodes != null)
            {
                return _cachedNodes;
            }

            var nodesToDraw = new Dictionary<int, NodeDrawElement>(genome.NodeGenes.Count);

            var inputNodes = genome.InputNodes;
            var inputIndex = 0;
            for (int y = -6; y <= 6; y++)
            {
                for (int x = -6; x <= 6; x++)
                {
                    var nodeToDraw = new NodeDrawElement
                    {
                        X = 70 + 10 * x,
                        Y = 70 + 10 * y
                    };
                    nodesToDraw.Add(inputNodes[inputIndex].Id, nodeToDraw);
                    inputIndex++;
                };
            }

            var biasNodeToDraw = new NodeDrawElement
            {
                X = 130,
                Y = 150
            };
            nodesToDraw.Add(inputNodes.Last().Id, biasNodeToDraw);

            for (int i = 0; i < genome.OutputNodes.Count; i++)
            {
                var outputNode = genome.OutputNodes[i];
                var nodeToDraw = new NodeDrawElement
                {
                    X = 500,
                    Y = 30 + 13 * i
                };
                nodesToDraw.Add(outputNode.Id, nodeToDraw);
            }

            foreach (var hiddenNode in genome.HiddenNodes)
            {
                var nodeToDraw = new NodeDrawElement
                {
                    X = 150,
                    Y = 80
                };
                nodesToDraw.Add(hiddenNode.Id, nodeToDraw);
            }

            var connectionGenesToDraw = genome.ConnectionGenes.Values.Where(cg => cg.IsEnabled);
            for (int i = 0; i < 4; i++)
            {
                foreach (var connectionGene in connectionGenesToDraw)
                {
                    var previousNode = connectionGene.PreviousNode;
                    var nextNode = connectionGene.NextNode;

                    var previousNodeToDraw = nodesToDraw[previousNode.Id];
                    var nextNodeToDraw = nodesToDraw[nextNode.Id];

                    if (previousNode.Type == NodeGeneType.Hidden)
                    {
                        previousNodeToDraw.X = 0.75f * previousNodeToDraw.X + 0.25f * nextNodeToDraw.X;
                        previousNodeToDraw.Y = 0.75f * previousNodeToDraw.Y + 0.25f * nextNodeToDraw.Y;

                        if (previousNodeToDraw.X >= nextNodeToDraw.X)
                        {
                            previousNodeToDraw.X -= 40;
                        }
                        if (previousNodeToDraw.X < 150)
                        {
                            previousNodeToDraw.X = 150;
                        }

                        if (previousNodeToDraw.X > 470)
                        {
                            previousNodeToDraw.X = 470;
                        }
                    }
                    if (nextNode.Type == NodeGeneType.Hidden)
                    {
                        nextNodeToDraw.X = 0.25f * previousNodeToDraw.X + 0.75f * nextNodeToDraw.X;
                        nextNodeToDraw.Y = 0.25f * previousNodeToDraw.Y + 0.75f * nextNodeToDraw.Y;

                        if (previousNodeToDraw.X >= nextNodeToDraw.X)
                        {
                            nextNodeToDraw.X += 40;
                        }
                        if (nextNodeToDraw.X < 150)
                        {
                            nextNodeToDraw.X = 150;
                        }

                        if (nextNodeToDraw.X > 470)
                        {
                            nextNodeToDraw.X = 470;
                        }
                    }
                }
            }

            _cachedNodes = nodesToDraw;
            return nodesToDraw;
        }

        private class NodeDrawElement
        {
            public float X { get; set; }
            public float Y { get; set; }
        }
    }

}
