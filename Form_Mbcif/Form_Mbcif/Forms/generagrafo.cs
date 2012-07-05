using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Graphviz4Net.Graphs;

namespace Form_Mbcif.Forms
{
    public class ElementoGrafo
    {
        private readonly Graph<ElementoGrafo> Graph;

        public ElementoGrafo(Graph<ElementoGrafo> Graph)
        {
            this.Graph = Graph;
        }

        public String nombre { get; set; }
        public int valor { get; set; }

        public ICommand RemoveCommand
        {
            get { return new RemoveCommandImpl(this); }
        }

        private class RemoveCommandImpl : ICommand
        {
            private ElementoGrafo elem;

            public RemoveCommandImpl(ElementoGrafo elem)
            {
                this.elem = elem;
            }

            public void Execute(object parameter)
            {
                this.elem.Graph.RemoveVertexWithEdges(this.elem);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }
    }

    public class DiamondArrow
    {
    }

    public class Arrow
    {
    }

    public class generagrafo : INotifyPropertyChanged
    {
        List<SubGraph<ElementoGrafo>> sg = new List<SubGraph<ElementoGrafo>>();

        public generagrafo()
        {
            var graph = new Graph<ElementoGrafo>();
            this.Graph = graph;
            this.Graph.Changed += GraphChanged;
        }

        public Graph<ElementoGrafo> Graph { get; private set; }

        public IEnumerable<string> nombreselementos
        {
            get { return this.Graph.AllVertices.Select(x => x.nombre); }
        }

        public void CreateElemento(Elemento elemento, Nivel n)
        {
            if (this.nombreselementos.Any(x => x == elemento.nombre))
            {
                // such a person already exists: there should be some validation message, but 
                // it is not so important in a demo
                return;
            }

            var p = new ElementoGrafo(this.Graph) { nombre = elemento.nombre, valor = (int)elemento.valor };
            int i = revisarsubgrafo(n);
            if (i != -1)
            {
                this.Graph.SubGraphs.ElementAt(i).AddVertex(p);
            }
            else
            {
                SubGraph<ElementoGrafo> sg2 = new SubGraph<ElementoGrafo>() { Label = n.nombre};
                sg2.AddVertex(p);
                this.Graph.AddSubGraph(sg2);
                if (existeenotrosubgrafo(sg2.Label))
                {
                    createlink(sg2, buscarsubgrafo(n.nombre));
                }
            }
        }

        private bool existeenotrosubgrafo(string nn)
        {
            foreach (SubGraph<ElementoGrafo> sgc in this.Graph.SubGraphs)
            {
                foreach (ElementoGrafo elg in sgc.Vertices)
                {
                    if (elg.nombre.Equals(nn))
                    {
                        return true;
                    }
                    
                }
            }
            return false;
        }

        
        private void createlink(SubGraph<ElementoGrafo> sg2, SubGraph<ElementoGrafo> sg3)
        {
            this.Graph.AddEdge(new Edge<SubGraph<ElementoGrafo>>(sg2, sg3));
        }

        private int revisarsubgrafo(Nivel n)
        {
            int i = 0;
            foreach(SubGraph<ElementoGrafo> sb in this.Graph.SubGraphs)
            {
                if(sb.Label.Equals(n.nombre))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public void Createsubgraph(Nivel n)
        {
            sg.Add(new SubGraph<ElementoGrafo>() {Label = n.nombre});
        }

        public void CreateEdge(String e1, String e2, String signo, String valor)
        {
            ElementoGrafo a = devuelveelemento(e1);
            ElementoGrafo b = devuelveelemento(e2);
            this.Graph.AddEdge(new Edge<ElementoGrafo>(a, b, new Arrow()) { Label = signo+" "+valor});
        }

        private ElementoGrafo devuelveelemento(String e)
        {
            ElementoGrafo ag = null;
            foreach (ElementoGrafo eg in this.Graph.AllVertices)
            {
                if (eg.nombre.Equals(e))
                {
                    ag = eg;
                }
            }
            return ag;
        }

        internal void removesubgraph(string p)
        {
            foreach (SubGraph<ElementoGrafo> s in this.Graph.SubGraphs)
            {
                if (s.Vertices.Count() == 0)
                {
                    this.Graph.RemoveSubGraph(s);
                    break;
                }
            }
        }

        public void cambiarelemento(ElementoGrafo l, int nuevovalor)
        {
            l.valor = nuevovalor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GraphChanged(object sender, GraphChangedArgs e)
        {
            this.RaisePropertyChanged("nombreselementos");
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        internal void createlink(string p, string p_2)
        {
            SubGraph<ElementoGrafo> ssg = buscarsubgrafo(p);
            SubGraph<ElementoGrafo> ssg2 = buscarsubgrafo(p);
            this.Graph.AddEdge(new Edge<SubGraph<ElementoGrafo>>(ssg, ssg2));
        }

        private SubGraph<ElementoGrafo> buscarsubgrafo(string p)
        {
            SubGraph<ElementoGrafo> sgeg = null;
            foreach (SubGraph<ElementoGrafo> sg in this.Graph.SubGraphs)
            {
                foreach (ElementoGrafo elg in sg.Vertices)
                {
                    if (elg.nombre.Equals(p))
                    {
                        sgeg = sg;
                    }

                }
            }
            return sgeg;
        }

        internal void removeelemento(string value)
        {
            foreach (ElementoGrafo el in this.Graph.AllVertices)
            {
                if (el.nombre.Equals(value))
                {
                    this.Graph.RemoveVertexWithEdges(el);
                    removesubgraph(value);
                    break;
                }
            }
        }

        internal void removeallvertex(string p)
        {
            foreach (SubGraph<ElementoGrafo> sgs in this.Graph.SubGraphs)
            {
                if (sgs.Label.Equals(p))
                {
                    foreach (ElementoGrafo elg in sgs.Vertices)
                    {
                        this.Graph.RemoveVertexWithEdges(elg);
                        break;
                    }
                    this.Graph.RemoveSubGraph(sgs);
                    break;
                }
            }
        }
    }
}
