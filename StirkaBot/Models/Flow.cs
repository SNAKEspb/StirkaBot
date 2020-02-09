using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StirkaBot.Models
{
    public class Flow
    {
        public Dictionary<String, Node> nodes = new Dictionary<string, Node>();
        public Flow add(Node node)
        {
            node.id = nodes.Count.ToString();
            nodes.Add(node.id, node);
            return this;
        }
        public Flow add(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                this.add(node);
            }
            return this;
        }
        public Node getNextNode(string nodeId, string linkId)
        {
            return nodes[nodeId].links[linkId].node;
        }

        public class Node
        {
            public string label;
            public string color;

            public string id;
            public Dictionary<String, Link> links = new Dictionary<string, Link>();

            public Node add(Link link)
            {
                link.id = links.Count.ToString();
                links.Add(link.id, link);
                return this;
            }
            public Node add(List<Link> links)
            {
                foreach (var link in links)
                {
                    this.add(link);
                }
                return this;
            }

        }

        public class Link
        {
            public string label;
            public string color;

            public string id;
            public Node node;
        }
    }
}
