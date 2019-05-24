using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace algoproject
{
    
    public partial class Form1 : Form
    {
        public struct nodes
        {
            public Int32 name;
            public float x;
            public float y;
            public double distance;
            public double time;
            public bool visited;
            public Int32 parent;
        }
        public struct info
        {
            public double distance;
            public double speed;
            public double time;

        };
        public struct query
        {
            public float xstart;
            public float ystart;
            public float xend;
            public float yend;
            public double radius;

        };

        public class getfiles
        {
            public List<string> GetAllFiles(string sDirt)
            {
                List<string> files = new List<string>();

                try
                {
                    foreach (string file in Directory.GetFiles(sDirt))
                    {
                        files.Add(file);
                    }
                    foreach (string fl in Directory.GetDirectories(sDirt))
                    {
                        files.AddRange(GetAllFiles(fl));
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }



                return files;
            }
        }
        public Form1()
        {
            InitializeComponent();


            readfolder(@"C:\Users\Lenovo\Desktop\New folder\[2] Medium Cases");



            int size_list = files.Count / 3;
            for (int i = 0; i < size_list; i++) //exact n , n:num of files
            {
                dic_edges.Clear(); //o(1)
                list_query.Clear();  //o(1)
                                     //list_vertix.Clear();  //o(1)

                nodes[] vertix = readfile(files[i], files[i + (2 * size_list)], files[i + size_list]);

                dic_temp.Clear(); //o(1)
                foreach (KeyValuePair<Int32, List<KeyValuePair<Int32, info>>> node in dic_edges) //o(e) , e = num of edges
                {
                    Int32 key = node.Key;
                    List<KeyValuePair<Int32, info>> lis = new List<KeyValuePair<Int32, info>>();
                    for (int c = 0; c < node.Value.Count; c++)
                    {
                        Int32 end_ = node.Value[c].Key;  //o(1)
                        info temp = new info();  //o(1)
                        temp.distance = node.Value[c].Value.distance; //o(1)
                        temp.speed = node.Value[c].Value.speed; //o(1)
                        temp.time = node.Value[c].Value.time; //o(1)


                        lis.Add(new KeyValuePair<Int32, info>(end_, temp)); //o(1)
                    }
                    dic_temp.Add(key, lis); //o(1)
                }


                priorityqueue.getdata(prority_queue, dic_edges, list_query);  //o(1)

                Stopwatch sw1 = new Stopwatch();
                sw1.Start();

                for (int j = 0; j < list_query.Count; j++)
                {

                    prority_queue.Clear();  //kda kda hy3ml over write 3lah

                    for (Int32 k = 0; k < vertix.Count(); k++) //o(v) , v: num of vertix
                    {
                        nodes p = new nodes();//o(1)
                        p.name = vertix[k].name;//o(1)
                        p.parent = vertix[k].parent;//o(1)
                        p.x = vertix[k].x;//o(1)
                        p.y = vertix[k].y;//o(1)
                        p.time = vertix[k].time;//o(1)
                        p.distance = vertix[k].distance;//o(1)
                        p.visited = vertix[k].visited;//o(1)
                        prority_queue.Add(p);//o(1)
                        //prority_queue[list_vertix[k].name] = p;
                    }


                    priorityqueue.getdata(prority_queue, dic_temp, list_query); //o(1)

                    priorityqueue.shortestpath(list_query[j].xstart, list_query[j].ystart, list_query[j].xend, list_query[j].yend, list_query[j].radius, i + 1);


                }

               // FileStream finalfile = new FileStream(@"C:\Users\Lenovo\Desktop\output\meduim" + (i+1) + ".txt", FileMode.Append, FileAccess.Write);
               // StreamWriter sw = new StreamWriter(finalfile);
               // sw.WriteLine(sw1.ElapsedMilliseconds + " ms");
                sw1.Stop(); //o(1)
                textBox1.AppendText(" "+ sw1.ElapsedMilliseconds + " ms ");


            }

        }
        public static List<nodes> prority_queue = new List<nodes>();
        public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> dic_edges = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
        public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> dic_temp = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
        public static List<query> list_query = new List<query>();
        public static List<string> files = new List<string>();
        public static int interval;
        
        public static List<List<PointF>> arraylist = new List<List<PointF>>();
        
        class priorityqueue
        {


            public static Dictionary<Int32, List<KeyValuePair<Int32, info>>> d_edges = new Dictionary<Int32, List<KeyValuePair<Int32, info>>>();
            public static List<query> list_query = new List<query>();
            public static List<nodes> priority_queue = new List<nodes>();
            public static List<nodes> list_vertics = new List<nodes>();




            public static void getdata(List<nodes> p_queue, Dictionary<Int32, List<KeyValuePair<Int32, info>>> edges, List<query> query) //o(1)
            {


                list_vertics = p_queue; //o(1)
                d_edges = edges; //o(1)

                list_query = query; //o(1)

            }

            public static Int32 Count { get { return priority_queue.Count; } }

            public static void insert_heap(nodes node) //o(log v)
            {
                priority_queue.Add(node);
                int i = Count - 1;

                while (i > 0)
                {
                    int p = (i - 1) / 2;
                    if (priority_queue[p].time <= node.time) { break; }

                    priority_queue[i] = priority_queue[p];
                    i = p;
                }

                if (Count > 0) priority_queue[i] = node;
            }
            public static void Min_heap(int i)//o(log v)
            {
                int r = 2 * i + 1; //o(1)
                int l = 2 * i; //o(1)
                int minimum;
                if (l <= priority_queue.Count - 1 && priority_queue[l].time < priority_queue[i].time) //o(1)
                {
                    minimum = l; //o(1)
                }
                else { minimum = i; } //o(1)
                if (r <= priority_queue.Count - 1 && priority_queue[r].time < priority_queue[minimum].time) //o(1)
                {
                    minimum = r;//o(1)
                }
                if (minimum != i)
                {
                    swap(i, minimum); //o(1)
                    Min_heap(minimum); //calculate recurance equation
                }

            }

            public static void swap(int i, int minimum) //o(1)
            {
                nodes newnode = new nodes();
                newnode = priority_queue[minimum]; //o(1)
                priority_queue[minimum] = priority_queue[i]; //o(1)
                priority_queue[i] = newnode; //o(1)
            }
            public static nodes heap_extract_min() //o(log v)
            {
                nodes min = Peek();
                nodes root = priority_queue[Count - 1];
                priority_queue.RemoveAt(Count - 1);

                int i = 0;
                while (i * 2 + 1 < Count)
                {
                    Int32 a = i * 2 + 1;
                    Int32 b = i * 2 + 2;
                    Int32 c = b < Count && priority_queue[b].time < priority_queue[a].time ? b : a;

                    if (priority_queue[c].time >= root.time) break;
                    priority_queue[i] = priority_queue[c];
                    i = c;
                }

                if (Count > 0) priority_queue[i] = root;
                return min;
            }
            public static nodes Peek()
            {
                if (Count == 0) throw new InvalidOperationException("Queue is empty.");
                return priority_queue[0];
            }


            public static void heap_dec_key()  //o(log v)
            {


                int index = priority_queue.Count - 1; //o(1)
                while (index > 1 && priority_queue[index / 2].time > priority_queue[index].time) //o(log v )
                {
                    swap(index, index / 2); //o(1)
                    index = index / 2; //o(1)
                }


            }



            public static void shortestpath(float xstart, float ystart, float xend, float yend, double r, int filenum)
            {


                Stopwatch sw2 = new Stopwatch();
                sw2.Start();

                nodes final = new nodes();  //o(1)

                final.name = list_vertics.Count;
                final.x = xend; //o(1)
                final.y = yend; //o(1)
                final.time = double.MaxValue; //o(1)
                final.distance = double.MaxValue; //o(1)
                final.visited = false;
                final.parent = -4; //o(1)




                nodes start = new nodes(); //o(1)
                start.name = -1; //o(1) 
                start.x = xstart; //o(1)
                start.y = ystart; //o(1)
                start.time = 0; //o(1)
                start.distance = 0; //o(1)
                start.parent = -5; //o(1)




                priority_queue.Add(start); //o(1)

                List<Int32> final_neg = new List<Int32>();
                for (int i = 0; i < list_vertics.Count; i++) //o(v)
                {

                    double start_node_dis, end_node_dis;
                    double xstart_xnode, xend_xnode;
                    double ystart_ynode, yend_ynode;
                    //----------------------start-------------------------
                    xstart_xnode = start.x - list_vertics[i].x; //o(1)
                    ystart_ynode = start.y - list_vertics[i].y; //o(1)
                    start_node_dis = Math.Sqrt((xstart_xnode * xstart_xnode) + (ystart_ynode * ystart_ynode)); //o(1)
                    if (start_node_dis <= r / 1000)  //o(1)
                    {
                        /*------------add start neighbours------------- */
                        info edge = new info();
                        int key_ = -1;
                        int end = list_vertics[i].name;
                        edge.distance = start_node_dis; //o(1)
                        edge.speed = 5; //o(1)
                        edge.time = (edge.distance / edge.speed) * 60; // time minutes //o(1)


                        if (d_edges.ContainsKey(key_) == true) //o(1)
                        {
                            List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                            d_edges.TryGetValue(key_, out old_list); //o(1)
                            old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                            d_edges[key_] = old_list; //o(1)

                        }
                        else
                        {
                            List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                            old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                            d_edges.Add(key_, old_list); //o(1)
                        }

                        /*5lst edaft al node l geran al start*/


                    }

                    /*end*/


                    xend_xnode = final.x - list_vertics[i].x; //o(1)
                    yend_ynode = final.y - list_vertics[i].y; //o(1)
                    end_node_dis = Math.Sqrt((xend_xnode * xend_xnode) + (yend_ynode * yend_ynode)); //o(1)
                    if (end_node_dis <= r / 1000) //o(1)
                    {

                        /*hdeef al end tkon geranhom*/

                        info edge = new info();
                        Int32 key_ = list_vertics[i].name; //o(1)
                        final_neg.Add(key_);//o(1)
                        Int32 end = final.name; //o(1)
                        edge.distance = end_node_dis; //o(1)
                        edge.speed = 5; //o(1)
                        edge.time = (edge.distance / edge.speed) * 60; // time minutes //o(1)


                        if (d_edges.ContainsKey(key_) == true) //o(1)
                        {
                            List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                            d_edges.TryGetValue(key_, out old_list); //o(1)
                            old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                            d_edges[key_] = old_list; //o(1)

                        }
                        else //o(1)
                        {
                            List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                            old_list.Add(new KeyValuePair<Int32, info>(end, edge)); //o(1)
                            d_edges.Add(key_, old_list); //o(1)
                        }

                        /*kda dafo al final b2t garthom*/
                    }//end if


                }//end loop

                //add final
                list_vertics.Add(final);//o(1) //m7d4 y7tha fo2 al for loop

                Int32 final_count = 0;

                while (priority_queue.Count > 0) //o(e log v)
                {
                    nodes extractnode = new nodes();
                    extractnode = heap_extract_min(); //o(log v)

                    if (extractnode.name == list_vertics.Count - 1) //o(1)
                    {
                        break;
                    }
                    if (extractnode.name == -1)
                    {

                    }
                    else if (list_vertics[extractnode.name].visited == false)
                    {
                        nodes m = new nodes();
                        m.x = list_vertics[extractnode.name].x;
                        m.y = list_vertics[extractnode.name].y;
                        m.name = list_vertics[extractnode.name].name;
                        m.time = list_vertics[extractnode.name].time;
                        m.distance = list_vertics[extractnode.name].distance;
                        m.parent = list_vertics[extractnode.name].parent;
                        m.visited = true;
                        list_vertics[extractnode.name] = m;



                    }
                    else if (list_vertics[extractnode.name].visited == true)
                    {
                        continue;
                    }
                    List<KeyValuePair<Int32, info>> neighbours = new List<KeyValuePair<Int32, info>>();


                    d_edges.TryGetValue(extractnode.name, out neighbours); //o(1)
                    Int32 j;

                    for (int k = 0; k < neighbours.Count; k++) // num of neighbours of extract node 
                    {

                        j = neighbours[k].Key;

                        if (list_vertics[j].time >= neighbours[k].Value.time + extractnode.time)//o(log v)
                        {

                            //update
                            nodes n = new nodes();
                            n.time = neighbours[k].Value.time + extractnode.time; //o(1)
                            n.distance = neighbours[k].Value.distance + extractnode.distance; //o(1)
                            n.parent = extractnode.name; //o(1)
                            n.name = list_vertics[j].name; //o(1)
                            n.x = list_vertics[j].x; //o(1)
                            n.y = list_vertics[j].y; //o(1)
                            list_vertics[j] = n; //o(1)

                            insert_heap(list_vertics[j]); //o(log v)


                        }


                    }


                }//end while


                sw2.Stop(); //o(1)
                            //Console.WriteLine(sw2.ElapsedMilliseconds);



                double walked = 0; //o(1)



                FileStream finalfile = new FileStream(@"C:\Users\Lenovo\Desktop\output\meduim" + filenum + ".txt", FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(finalfile);


                Stack<nodes> stack_nodes = new Stack<nodes>();
                List<PointF> listpoint = new List<PointF>();
                walked = list_vertics[list_vertics.Count - 1].distance - list_vertics[list_vertics[list_vertics.Count - 1].parent].distance;
                Int32 index = list_vertics.Count - 1;

                while (true)
                {
                    if (list_vertics[index].parent != -1)
                    {
                        stack_nodes.Push(list_vertics[index]);
                        index = list_vertics[index].parent;
                    }
                    else
                    {
                        walked += list_vertics[index].distance;
                        break;
                    }


                }
                //Console.WriteLine("the path");
                while (stack_nodes.Count != 0)
                {
                   
                    nodes nodeparent = new nodes();
                    nodeparent = stack_nodes.Pop();
                    sw.Write(nodeparent.parent + " ");

                    PointF p = new PointF();
                    p.X = nodeparent.x;
                    p.Y = nodeparent.y;
                    listpoint.Add(p);
                    

                }
                arraylist.Add(listpoint);

                /*-------------------------------------------------------------------------*/
                sw.WriteLine();



                sw.WriteLine(Math.Round(list_vertics[list_vertics.Count - 1].time, 2) + " mins");
                sw.WriteLine(Math.Round(list_vertics[list_vertics.Count - 1].distance, 2) + " km");
                sw.WriteLine(Math.Round(walked, 2) + " km");
                sw.WriteLine(Math.Round((list_vertics[list_vertics.Count - 1].distance - walked), 2) + " km");//car distance

                //sw.WriteLine(sw2.ElapsedMilliseconds + " ms");
                sw.WriteLine();
                sw.Close();



                priority_queue.Clear(); //o(1)
                d_edges.Remove(-1);//ms7t al start
                                   /*hms7 final mn kol node*/


                for (int i = 0; i < final_neg.Count; i++)
                {

                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                    d_edges.TryGetValue(final_neg[i], out old_list); //o(1)
                    old_list.RemoveAt(old_list.Count - 1);//o(1)

                    d_edges[final_neg[i]] = old_list; //o(1)
                }

                final_neg.Clear();

            }

        }

        public static void readfolder(string foldername)
        {
            getfiles get = new getfiles();
            files = get.GetAllFiles(foldername);

            int size_list = files.Count / 3;


        }

        public static nodes[] readfile(string map, string query, string output)
        {

            FileStream file = new FileStream(@map, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);


            int vertices;

            //int wronganswers = 0;
            int edges;

            string line = sr.ReadLine();
            vertices = int.Parse(line);

            nodes[] list_vertix = new nodes[vertices];

            //prority_queue.Add(dummy);

            for (int i = 0; i < vertices; i++)
            {
                line = sr.ReadLine();
                string[] lineparts = line.Split(' ');
                nodes n = new nodes();
                n.name = Int32.Parse(lineparts[0]);
                n.distance = double.MaxValue;
                n.time = double.MaxValue;
                n.parent = -4;
                n.visited = false;
                n.x = float.Parse(lineparts[1]);
                n.y = float.Parse(lineparts[2]);
                //prority_queue.Add(n);
                //list_vertix.Add(n);t3del


                list_vertix[n.name] = n;

            }


            //dictionary of edges


            line = sr.ReadLine();
            edges = int.Parse(line);


            for (int i = 0; i < edges; i++)//e
            {
                line = sr.ReadLine();
                string[] lineparts = line.Split(' ');

                info edge = new info();

                Int32 key_ = Int32.Parse(lineparts[0]);
                Int32 end = Int32.Parse(lineparts[1]);
                edge.distance = double.Parse(lineparts[2]);
                edge.speed = double.Parse(lineparts[3]);
                edge.time = (edge.distance / edge.speed) * 60; // time minutes


                if (dic_edges.ContainsKey(key_) == true)
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                    dic_edges.TryGetValue(key_, out old_list); // return bool (true or false) (true return value)
                    old_list.Add(new KeyValuePair<Int32, info>(end, edge));
                    dic_edges[key_] = old_list;

                }
                else
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                    old_list.Add(new KeyValuePair<Int32, info>(end, edge));
                    dic_edges.Add(key_, old_list);
                }

                //______________________//
                // bnzbt mn al end ll start

                if (dic_edges.ContainsKey(end) == true)
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();
                    dic_edges.TryGetValue(end, out old_list); // return bool (true or false) (true return value)
                    old_list.Add(new KeyValuePair<Int32, info>(key_, edge));
                    dic_edges[end] = old_list;
                }
                else
                {
                    List<KeyValuePair<Int32, info>> old_list = new List<KeyValuePair<Int32, info>>();

                    old_list.Add(new KeyValuePair<Int32, info>(key_, edge));
                    dic_edges.Add(end, old_list);
                }


            }

            sr.Close();
            //_____________________________________________________________//

            FileStream file1 = new FileStream(@query, FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(file1);

            int num_queries;

            line = sr1.ReadLine();
            num_queries = int.Parse(line);



            for (int i = 0; i < num_queries; i++)
            {
                line = sr1.ReadLine();
                string[] lineparts = line.Split(' ');
                query quer = new query();

                quer.xstart = float.Parse(lineparts[0]);
                quer.ystart = float.Parse(lineparts[1]);
                quer.xend = float.Parse(lineparts[2]);
                quer.yend = float.Parse(lineparts[3]);
                quer.radius = double.Parse(lineparts[4]);


                list_query.Add(quer);

            }
            sr1.Close();
            return list_vertix;//t3deel

        }
        private void show_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                int a = Convert.ToInt32(textBox1.Text);
                List<PointF> listpoint = new List<PointF>();
                if (a >= 0 && a <= arraylist.Count)
                {
                    listpoint = arraylist[a];

                    PointF[] list = new PointF[listpoint.Count];
                    if (arraylist.Count >= 1000)
                    {
                        // this is meduim or larage
                        for (int i = 0; i < listpoint.Count(); i++)
                        {

                            PointF pp = new PointF();
                            pp.X = listpoint[i].X / 10;
                            pp.Y = listpoint[i].Y / 10;
                            list[i] = pp;
                        }
                    }
                    else
                    {
                        // this is smale
                        for (int i = 0; i < listpoint.Count(); i++)
                        {
                            

                            PointF pp = new PointF();
                            pp.X = listpoint[i].X*10 ;
                            pp.Y = listpoint[i].Y*10 ;
                            
                            if (pp.X < 100)
                            {
                                pp.X = pp.X * 100;
                                pp.Y = pp.Y * 100;
                            }
                           
                            list[i] = pp;
                        }
                    }
                    
                    CreateGraphics().Clear(Color.LightBlue);
                    this.CreateGraphics().DrawLines(new Pen(Brushes.Blue, 2), list);
                }
            }
            
            
         
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
    }
}
