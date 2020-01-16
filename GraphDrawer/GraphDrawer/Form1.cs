using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace GraphDrawer
{
    public partial class Form1 : Form
    {
        bool barGraph = true;
        bool unUniqueWords = true;
        string filePath = "";

        public Form1()
        {
            InitializeComponent();
            DrawGraph();

        }
        private void DrawGraph()
        {
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();

            if (barGraph)
            {
                BarGraph();
            }
            else
            {
                PieGraph();
            }
        }

        private void GraphType(string graphType)
        {
            if (graphType == "bar")
            {
                if (!barGraph)
                {
                    barGraph = true;
                    barToolStripMenuItem.Checked = true;
                    pieToolStripMenuItem.Checked = false;
                }
            }
            if (graphType == "pie")
            {
                if (barGraph)
                {
                    barGraph = false;
                    barToolStripMenuItem.Checked = false;
                    pieToolStripMenuItem.Checked = true;
                }
            }
        }

        private void BarGraph()
        {
            GraphPane wordCounterGraph = zedGraphControl1.GraphPane;
            wordCounterGraph.Title.Text = "Word Counter for Text Files";
            wordCounterGraph.XAxis.Title.Text = "Unique Words";
            wordCounterGraph.YAxis.Title.Text = "Word Count";

            List<KeyValuePair<string, int>> wordCount = BackendWordProcessing.Process(filePath);
            int uniqueWordsTotal = 0;
            int uniqueWordsCounter = 0;

            for (int i = 0; i < wordCount.Count; i++)
            {
                if (wordCount[i].Value == 1)
                {
                    uniqueWordsTotal++;
                }
            }

            if (unUniqueWords)
            {
                double[] x = new double[wordCount.Count+1 - uniqueWordsTotal];
                double[] y = new double[wordCount.Count+1 - uniqueWordsTotal];

                for (int i = 0; i < wordCount.Count; i++)
                {
                    if (wordCount[i].Value != 1)
                    {
                        x[i - uniqueWordsCounter] = i - uniqueWordsCounter;
                        y[i - uniqueWordsCounter] = wordCount[i - uniqueWordsCounter].Value;
                        TextObj wordName = new TextObj($"{wordCount[i].Key}", i, -1);
                        TextObj wordValue = new TextObj($"{wordCount[i].Value}", i, wordCount[i].Value + 1);

                        wordName.FontSpec.Border.IsVisible = false;
                        wordName.FontSpec.Fill.IsVisible = false;
                        wordName.FontSpec.Angle = 75;
                        wordName.FontSpec.Size = 9;
                        wordName.FontSpec.IsBold = true;

                        wordValue.FontSpec.Border.IsVisible = false;
                        wordValue.FontSpec.Fill.IsVisible = false;
                        wordValue.FontSpec.Size = 10;
                        wordValue.FontSpec.IsBold = true;

                        wordCounterGraph.GraphObjList.Add(wordName);
                        wordCounterGraph.GraphObjList.Add(wordValue);
                    }
                    else
                    {
                        uniqueWordsCounter++;
                    }
                }
                x[wordCount.Count - uniqueWordsTotal] = wordCount.Count - uniqueWordsCounter;
                y[wordCount.Count - uniqueWordsTotal] = 1;

                TextObj uniqueWordName = new TextObj("UNIQUE-WORDS", wordCount.Count - uniqueWordsCounter, -1);
                TextObj uniqueWordValue = new TextObj($"{uniqueWordsCounter}", wordCount.Count - uniqueWordsCounter, 2);

                uniqueWordName.FontSpec.Border.IsVisible = false;
                uniqueWordName.FontSpec.Fill.IsVisible = false;
                uniqueWordName.FontSpec.Angle = 75;
                uniqueWordName.FontSpec.Size = 9;
                uniqueWordName.FontSpec.IsBold = true;

                uniqueWordValue.FontSpec.Border.IsVisible = false;
                uniqueWordValue.FontSpec.Fill.IsVisible = false;
                uniqueWordValue.FontSpec.Size = 10;
                uniqueWordValue.FontSpec.IsBold = true;


                wordCounterGraph.GraphObjList.Add(uniqueWordName);
                wordCounterGraph.GraphObjList.Add(uniqueWordValue);
                wordCounterGraph.AddBar("words", x, y, Color.Red);
            }
            else
            {
                double[] x = new double[wordCount.Count];
                double[] y = new double[wordCount.Count];

                for (int i = 0; i < wordCount.Count; i++)
                {
                    x[i] = i;
                    y[i] = wordCount[i].Value;

                    TextObj wordName = new TextObj($"{wordCount[i].Key}", i, -1);
                    TextObj wordValue = new TextObj($"{wordCount[i].Value}", i, wordCount[i].Value + 1);

                    wordName.FontSpec.Border.IsVisible = false;
                    wordName.FontSpec.Fill.IsVisible = false;
                    wordName.FontSpec.Angle = 75;
                    wordName.FontSpec.Size = 9;
                    wordName.FontSpec.IsBold = true;

                    wordValue.FontSpec.Border.IsVisible = false;
                    wordValue.FontSpec.Fill.IsVisible = false;
                    wordValue.FontSpec.Angle = 0;
                    wordValue.FontSpec.Size = 10;
                    wordValue.FontSpec.IsBold = true;

                    wordCounterGraph.GraphObjList.Add(wordName);
                    wordCounterGraph.GraphObjList.Add(wordValue);
                }
                wordCounterGraph.AddBar("words", x, y, Color.Red);
            }
            zedGraphControl1.AxisChange();

        }

        private void PieGraph()
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openFile.FileName;
                DrawGraph();
            }
        }

        private void uniqueWordsVisibleToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            if (!unUniqueWords)
            {
                unUniqueWords = true;
            }
            else
            {
                unUniqueWords = false;
            }

            DrawGraph();

        }

        private void pieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphType("pie");
            DrawGraph();
        }

        private void barToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphType("bar");
            DrawGraph();
        }
    }
}
