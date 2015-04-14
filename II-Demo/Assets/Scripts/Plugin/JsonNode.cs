/**
  
    Copyright [2014] [AlkingSun @ Morln NJ (南京魔韵孙爱晶) ]

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJson
{
    #region SimpleJsonException
    public class SimpleJsonException : Exception
    {
        public SimpleJsonException(string message)
            : base(message)
        {

        }

    }
    #endregion

    #region NodeType
    /// <summary>
    /// JsonNode的类型.
    /// <p>注意：只有最低层的节点的类型才是值类型。</p>
    /// <ul>
    /// <li>Null</li>
    /// <li>Object，物体类型</li>
    /// <li>Array，数组类型</li>
    /// <li>Value,值类型</li>
    /// </ul>
    /// </summary>
    public enum NodeType
    {
        Null,
        Object,
        Array,
        Value,
    }
    #endregion
    /// <summary>
    /// ValueType
    /// <p>注意：必须是最低层的节点，才有Value</p>
    /// <ul>
    /// <li>Null，空类型</li>
    /// <li>Boolean</li>
    /// <li>Int</li>
    /// <li>Long</li>
    /// <li>Null</li>
    /// <li>Double</li>
    /// <li>String</li>
    /// </ul>
    /// </summary>
    #region ValueType
    public enum ValueType
    {
        /// <summary>
        /// 
        /// </summary>
        Null,
        /// <summary>
        /// bool
        /// </summary>
        Boolean,
        /// <summary>
        /// int
        /// </summary>
        Int,
        /// <summary>
        /// long
        /// </summary>
        Long,
        /// <summary>
        /// double
        /// </summary>
        Double,
        /// <summary>
        /// string
        /// </summary>
        String,
    }
    #endregion


    /// <summary>
    /// <h1>Json节点</h1>
    /// 1.0.0 
    /// AlkingSun @ Morln
    /// 2014-1-4 13:30
    /// 
    /// 
    /// 
    /// </summary>
    /// 
    /// 
    /// <example>
    /// 第一部分：解析json
    /// <p>==========我是华丽的分割线==========</p>
    /// <p>string json = "{\"name1\":123,\"name2\":true,\"name3\":1.23,\"name4\":\"hello, world.\"}";</p>
    /// <p>JsonNode root = JsonNode.FromJson(json);</p>
    /// <p>Assert.AreEqual(123,root["name1"].Value);</p>
    /// <p>Assert.AreEqual(true,root["name2"].Value);</p>
    /// <p>Assert.AreEqual(1.23,root["name3"].Value);</p>
    /// <p>Assert.AreEqual("hello, world.",root["name4"].Value);</p>
    /// <p>==========我是华丽的分割线==========</p>
    ///
    /// 
    /// <p>string json = "{\"name1\":123,\"name2\":true,\"name3\":1.23,\"name4\":\"hello, world.\"}";</p>
    /// <p>JsonNode root = JsonNode.FromJson(json);</p>
    /// <p>JsonNode subNode = root["name1"];</p>
    /// <p>int intergerValue = subNode;</p>
    /// <p>Assert.AreEqual(123,intergerValue);</p>
    /// <p> subNode = root["name2"];</p>
    /// <p>bool bValue = subNode;</p>
    /// <p>Assert.AreEqual(true,bValue);</p>
    /// 
    /// <p>subNode = root["name3"];</p>
    /// <p>double dValue = subNode;</p>
    /// <p>Assert.AreEqual(1.23, dValue);</p>
    /// 
    /// <p>subNode = root["name4"];</p>
    /// <p>string stringValue = subNode;</p>
    /// <p>Assert.AreEqual("hello, world.", stringValue);</p>
    /// 
    /// <p>==========我是华丽的分割线==========</p>
    /// <p>string json = "[  123,  true,  1.23,   \"hello, world.\"]";</p>
    /// <p>JsonNode root = JsonNode.FromJson(json);</p>
    /// <p>Assert.AreEqual(123, root[0].Value);</p>
    /// <p>Assert.AreEqual(true, root[1].Value);</p>
    /// <p> Assert.AreEqual(1.23, root[2].Value);</p>
    /// <p>Assert.AreEqual("hello, world.", root[3].Value);</p>
    /// <p>==========我是华丽的分割线==========</p>
    /// <p>第二部分：构建json</p>
    /// <p>JsonNode root = new JsonNode(NodeType.Object);</p>
    /// <p>JsonNode subNode = 123;</p>
    /// <p>root.AddSubNode("name1",subNode);</p>
    /// <p>subNode = true;</p>
    /// <p>root.AddSubNode("name2", subNode);</p>
    /// <p>subNode = false;</p>
    /// <p>root.AddSubNode("name3", subNode);</p>
    /// <p> subNode = "hello world.";</p>
    /// <p>root.AddSubNode("name4", subNode);</p>
    /// <p>subNode = null;</p>
    /// <p>root.AddSubNode("name5", subNode);</p>
    /// <p>subNode = 1.23;</p>
    /// <p>root.AddSubNode("name6", subNode);</p>
    /// <p>Console.WriteLine(root.ToJson());</p>
    /// <p>输出：{"name1":123,"name2":true,"name3":false,"name4":"hello world.","name5":null,"name6":1.23}</p>
    /// <p>==========我是华丽的分割线==========</p>
    /// <p>JsonNode root = new JsonNode(NodeType.Array);</p>
    /// <p>JsonNode subNode = 123;</p>
    /// <p> root.AddNode(subNode);</p>
    /// <p>subNode = true;</p>
    /// <p>root.AddNode(subNode);</p>
    /// <p>subNode = "hello world.";</p>
    /// <p>root.AddNode(subNode);</p>
    /// <p>subNode = null;</p>
    /// <p>root.AddNode(subNode);</p>
    /// <p>subNode = 1.234;</p>
    /// <p>root.AddNode(subNode);</p>
    /// <p>Console.WriteLine(root.ToJson());</p>
    /// <p>输出：[123,true,"hello world.",null,1.234]</p>
    /// </example>
    public class JsonNode
    {

        #region property

        public NodeType NodeType { get; private set; }

        public ValueType ValueType { get; private set; }


        /// <summary>
        /// <p>value</p>
        /// <p>可以是bool,int,long,double,string,null...</p>
        /// </summary>
        private string StringValue { get; set; }
        /// <summary>
        /// <p>可以是bool,int,long,double,string...</p>
        /// <p>注意：只有最低层的节点才有Value</p>
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public JsonNode SuperNode { get; private set; }
        /// <summary>
        /// 如果本节点的类型是列表。才有Nodes
        /// </summary>
        private List<JsonNode> Nodes { get; set; }

        /// <summary>
        /// 
        /// 如果该节点的类型是Array。返回子节点的数量
        /// 否则会抛异常
        /// 
        /// <exception cref="SimpleJsonException">该Node不是一个Array</exception>
        /// </summary>
        public int ElementNodeCount
        {
            get
            {
                if (NodeType == NodeType.Array)
                {

                }
                else
                {
                    throw new SimpleJsonException("该Node不是一个Array");
                }
                return Nodes.Count;
            }
        }

        /// <summary>
        /// 添加节点。
        /// <p>注意：本身的节点类型是Array</p>
        /// </summary>
        /// <exception cref="SimpleJsonException">该节点的类型必须是Array</exception>
        /// <param name="node"></param>
        public void AddNode(JsonNode node)
        {
            if (NodeType == NodeType.Array)
            {

            }
            else
            {
                throw new SimpleJsonException(string.Format("该节点的类型不是Array"));
            }
            if (node == null)
            {
                node = new JsonNode();
                node.NodeType = NodeType.Value;
            }
            node.SuperNode = this;
            Nodes.Add(node);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="SimpleJsonException">该Node不是一个Array</exception>
        /// <returns>
        /// <p>node，index在范围之内</p>
        /// <p>null，index不在范围之内</p>
        /// </returns>
        public JsonNode this[int index]
        {
            get
            {
                if (NodeType != NodeType.Array)
                {
                    throw new SimpleJsonException("该Node不是一个Array");
                }
                if (Nodes.Count > index && index >= 0)
                {
                    return Nodes[index];
                }
                return null;
            }
        }
        /// <summary>
        /// 只有本节点的类型是Object才有NodeDic
        /// </summary>
        private Dictionary<string, JsonNode> NodeDic { get; set; }
        /// <summary>
        /// 如果该节点的类型是Object。返回子节点的数量
        /// 否则会抛异常
        /// <exception cref="SimpleJsonException">该Node不是一个Object</exception>
        /// </summary>
        public int SubNodesCount
        {
            get
            {
                if (NodeType == NodeType.Object)
                {

                }
                else
                {
                    throw new SimpleJsonException("该Node不是一个Object or ArrayElementObject");
                }
                return NodeDic.Count;
            }
        }

        /// <summary>
        /// 添加子节点
        /// <p>注意：该节点是Object</p>
        /// </summary>
        /// <exception cref="SimpleJsonException">该Node不是一个Object,被添加的Node不能是ArrayElemnt,该节点已经存在子节点对应的名称了</exception>
        /// <param name="name"></param>
        /// <param name="node">子节点</param>
        public void AddSubNode(string name, JsonNode node)
        {
            if (NodeType == NodeType.Object)
            {

            }
            else
            {
                throw new SimpleJsonException("该Node不是一个Object or ArrayElementObject");
            }

            if (NodeDic.ContainsKey(name))
            {
                throw new SimpleJsonException(string.Format("该节点已经存在子节点对应的名称了：{0}", name));
            }

            if (node == null)
            {
                node = new JsonNode();
                node.NodeType = NodeType.Value;
            }
            node.SuperNode = this;
            NodeDic.Add(name, node);
        }
        /// <summary>
        /// 返回所有子节点的名称
        ///  <p>注意：该节点类型是Object</p>
        /// </summary>
        public List<string> SubNodeNames
        {
            get
            {
                if (NodeType == NodeType.Object)
                {

                }
                else
                {
                    throw new SimpleJsonException("该Node不是一个Object or ArrayElementObject");
                }

                List<string> result = new List<string>();
                foreach (KeyValuePair<string, JsonNode> keyValuePair in NodeDic)
                {
                    result.Add(keyValuePair.Key);
                }
                return result;
            }
        }

        /// <summary>
        /// 得到子节点
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="SimpleJsonException">该Node不是一个Object，name  is null or empty</exception>
        /// 
        /// <returns></returns>
        public JsonNode this[string name]
        {
            get
            {
                if (NodeType != NodeType.Object)
                {
                    throw new SimpleJsonException("该Node不是一个Object");
                }
                if (string.IsNullOrEmpty(name))
                {
                    throw new SimpleJsonException(string.Format("param : name  is null or empty"));
                }
                if (NodeDic.ContainsKey(name))
                {
                    return NodeDic[name];
                }
                return null;
            }
        }




        #endregion

        #region implicit
        public static implicit operator bool(JsonNode node)
        {
            if (node == null)
            {
                throw new SimpleJsonException("node can not be null");
            }

            if (node.ValueType == ValueType.Boolean)
            {
                bool result = bool.Parse(node.StringValue);
                return result;
            }
            throw new SimpleJsonException("this node is't a bool");
        }

        public static implicit operator JsonNode(bool value)
        {
            return new JsonNode(value);
        }

        public static implicit operator int(JsonNode node)
        {
            if (node == null)
            {
                throw new SimpleJsonException("node can not be null");
            }
            if (node.ValueType == ValueType.Int)
            {
                int result = int.Parse(node.StringValue);
                return result;
            }
            throw new SimpleJsonException("this node is't a int");
        }
        public static implicit operator JsonNode(int value)
        {
            var node = new JsonNode(value);

            return node;
        }



        public static implicit operator long(JsonNode node)
        {
            if (node == null)
            {
                throw new SimpleJsonException("node can not be null");
            }
            if (node.ValueType == ValueType.Long || node.ValueType == ValueType.Int)
            {
                long result = long.Parse(node.StringValue);
                return result;
            }
            throw new SimpleJsonException("this node is't a long");
        }
        public static implicit operator JsonNode(long value)
        {
            return new JsonNode(value);
        }



        public static implicit operator double(JsonNode node)
        {
            if (node == null)
            {
                throw new SimpleJsonException("node can not be null");
            }
            if (node.ValueType == ValueType.Double)
            {
                double result = double.Parse(node.StringValue);
                return result;
            }
            throw new SimpleJsonException("this node is't a double");
        }
        public static implicit operator JsonNode(double value)
        {
            return new JsonNode(value);
        }


        public static implicit operator string(JsonNode node)
        {
            if (node == null)
            {
                throw new SimpleJsonException("node can not be null");
            }
            if (node.ValueType == ValueType.String)
            {
                var result = node.StringValue;

                return result;
            }
            throw new SimpleJsonException("this node is't a string");
        }
        public static implicit operator JsonNode(string value)
        {
            return new JsonNode(value);
        }


        #endregion

        #region constructor
        /// <summary>
        /// 构造函数。
        /// <p>
        /// 默认NodeType(Null)和默认ValueType(Null)
        /// </p>
        /// </summary>
        private JsonNode()
        {
            NodeType = NodeType.Null;
            ValueType = ValueType.Null;
            StringValue = null;
            Value = null;
            Nodes = new List<JsonNode>();
            NodeDic = new Dictionary<string, JsonNode>();
        }
        /// <summary>
        /// 构造函数
        /// <p>根节点。类型只能是Object和Array</p>
        /// </summary>
        /// <param name="nodeType"></param>
        public JsonNode(NodeType nodeType)
            : this()
        {
            if (nodeType == NodeType.Object || nodeType == NodeType.Array)
            {

            }
            else
            {
                throw new SimpleJsonException("Node Type can only be Object or Array");
            }
            NodeType = nodeType;
        }


        private JsonNode(object value, NodeType nodeType, ValueType valueType)
            : this()
        {
            StringValue = value.ToString();
            if (value is bool)
            {
                StringValue = StringValue.ToLower();
            }
            Value = value;
            NodeType = nodeType;
            ValueType = valueType;
        }



        public JsonNode(bool value)
            : this(value, NodeType.Value, ValueType.Boolean)
        {
        }



        public JsonNode(int value)
            : this(value, NodeType.Value, ValueType.Int)
        {
        }



        public JsonNode(long value)
            : this(value, NodeType.Value, ValueType.Long)
        {
        }


        public JsonNode(double value)
            : this(value, NodeType.Value, ValueType.Double)
        {
        }

        public JsonNode(string value)
            : this(value, NodeType.Value, ValueType.String)
        {

        }

        private JsonNode(string value, string json, int offset)
            : this()
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new SimpleJsonException("value can't be null or empty");
            }

            bool find = false;

            if (!find)
            {
                bool boolValue;
                if (bool.TryParse(value, out boolValue))
                {
                    find = true;
                    NodeType = NodeType.Value;
                    ValueType = ValueType.Boolean;
                    StringValue = boolValue.ToString().ToLower();
                    Value = boolValue;
                }
            }

            if (!find)
            {
                int intValue;
                if (int.TryParse(value, out intValue))
                {
                    find = true;
                    NodeType = NodeType.Value;
                    ValueType = ValueType.Int;
                    Value = intValue;
                    StringValue = intValue.ToString();
                }
            }

            if (!find)
            {
                long longValue;
                if (long.TryParse(value, out longValue))
                {
                    find = true;
                    NodeType = NodeType.Value;
                    ValueType = ValueType.Long;
                    StringValue = longValue.ToString();
                    Value = longValue;
                }
            }

            if (!find)
            {
                double doubleValue;
                if (double.TryParse(value, out doubleValue))
                {
                    find = true;
                    NodeType = NodeType.Value;
                    ValueType = ValueType.Double;
                    StringValue = doubleValue.ToString();
                    Value = doubleValue;

                }
            }
            if (!find)
            {

                NodeType = NodeType.Value;
                ValueType = ValueType.String;
                string content;

                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    content = value.Substring(1, value.Length - 2);
                }
                else if (value.Equals("null"))
                {
                    content = null;
                    ValueType = ValueType.Null;
                }
                else
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("value {3} is invalid ,json:{0},offset :{1},char begin is \'{2}\',which should be \'\"\'(string value start)", json, error, json[error], value));
                }

                StringValue = content;
                Value = content;

            }

        }

        #endregion

        #region ParseFrom


        public static JsonNode FromJson(string json)
        {

            if (string.IsNullOrEmpty(json))
            {
                throw new SimpleJsonException("json can't be null or empty");
            }
            int offset = 0;
            JsonNode node;

            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                node = ParseObject(json, ref offset);
                if (offset < json.Length - 1)
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("json has ended at json: {0} ,offset: {1}, which char is \'{2}\'", json, error, json[error]));
                }
                return node;
            }

            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                node = ParseArray(json, ref offset);
                if (offset < json.Length - 1)
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("json has ended at json: {0} ,offset: {1}, which char is \'{2}\'", json, error, json[error]));
                }
                return node;
            }
            throw new SimpleJsonException(string.Format("json format error"));

        }

        public string ToJson()
        {

            return ToString();

        }


        private static JsonNode ParseObject(string json, ref int offset)
        {
            JsonNode result = new JsonNode(NodeType.Object);
            int originOffset = offset;
            char c;
            while (true)
            {
                c = NextUnBlankControlChar(json, ref offset);

                if (c.Equals(','))
                {
                    continue;
                }
                if (c.Equals('}'))
                {
                    break;
                }
                if (!c.Equals('"'))
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("json:{0},offset:{1} which char is \'{2}\', but should be \'\"\'(name start)", json, error, json[error]));
                }

                string name = ParseName(json, ref offset);
                //找到第一个非空白和控制字符,这是后应该是:，名字和值的分隔符
                c = NextUnBlankControlChar(json, ref offset);
                if (!c.Equals(':'))
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("json:{0},offset:{1} which char is \'{2}\',but should be name and value separator char(\':\')", json, error, json[error]));
                }
                //找到第一个非空白和控制字符,这是后应该是值的开头
                c = NextUnBlankControlChar(json, ref offset);
                JsonNode node;
                if (c.Equals('{'))
                {
                    node = ParseObject(json, ref offset);
                }
                else if (c.Equals('['))
                {
                    node = ParseArray(json, ref offset);

                }
                else
                {
                    node = ParseValue(json, ref offset);
                }
                result.AddSubNode(name, node);
            }

            if (!json[offset].Equals('}'))
            {
                int error = offset;
                throw new SimpleJsonException(string.Format("json:{0},offset:{1} which char  is \'{2}\', but should be object end", json, error, json[offset]));
            }

            if (offset < json.Length - 1)
            {
                int cur = offset;
                char nextChar = NextUnBlankControlCharWithNoRef(json, cur);

                if (nextChar.Equals(',') || nextChar.Equals('}') || nextChar.Equals(']'))
                {

                }
                else
                {

                    throw new SimpleJsonException(string.Format("json:{0},offset:{1}, object string next unblank char is \'{2}\',but should be \',\' ,obj end or \']\'(array end)", json, originOffset, nextChar));
                }
            }
            return result;
        }

        private static JsonNode ParseArray(string json, ref int offset)
        {
            JsonNode result = new JsonNode(NodeType.Array);
            int originOffset = offset;
            char c;
            while (true)
            {

                c = NextUnBlankControlChar(json, ref offset);

                if (c.Equals(','))
                {
                    continue;

                } if (c.Equals(']'))
                {
                    break;
                }



                JsonNode node;
                if (c.Equals('{'))
                {
                    node = ParseObject(json, ref offset);
                }
                else if (c.Equals('['))
                {
                    node = ParseArray(json, ref offset);

                }
                else
                {
                    node = ParseValue(json, ref offset);
                }
                result.AddNode(node);
            }


            if (!json[offset].Equals(']'))
            {
                int error = offset;
                throw new SimpleJsonException(string.Format("json:{0},offset:{1} which char is \'{2}\',but should be \'[\' (array end)", json, error, json[error]));
            }
            if (offset < json.Length - 1)
            {
                int cur = offset;
                char nextChar = NextUnBlankControlCharWithNoRef(json, cur);

                if (nextChar.Equals(',') || nextChar.Equals('}') || nextChar.Equals(']'))
                {

                }
                else
                {

                    throw new SimpleJsonException(string.Format("json:{0},offset:{1}, array string next unblank char is \'{2}\',but should be \',\' ,obj end or \']\'(array end)", json, originOffset, nextChar));
                }
            }
            return result;
        }

        /// <summary>
        /// 解析Value
        /// 
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static JsonNode ParseValue(string json, ref int offset)
        {
            int originOffset = offset;

            bool isString = json[offset].Equals('"');

            if (isString)
            {
                offset++;
            }



            while (offset < json.Length)
            {

                if (isString)
                {
                    if (json[offset].Equals('"') && !json[offset - 1].Equals('\\'))
                    {

                        break;
                    }
                }
                else
                {
                    if (json[offset + 1].Equals(',') || json[offset + 1].Equals('}') || json[offset + 1].Equals(']'))
                    {
                        break;
                    }
                }
                offset++;
            }
            if (isString)
            {
                if (!json[offset].Equals('"'))
                {
                    int error = offset;
                    throw new SimpleJsonException(string.Format("json:{0},offset:{1} which char is \'{2}\', but should be \'\"\' (string value end)", json, error, json[error]));

                }
            }


            if (offset < json.Length - 1)
            {
                int cur = offset;
                char nextChar = NextUnBlankControlCharWithNoRef(json, cur);

                if (nextChar.Equals(',') || nextChar.Equals('}') || nextChar.Equals(']'))
                {

                }
                else
                {

                    throw new SimpleJsonException(string.Format("json:{0},offset:{1}, value string next unblank char is \'{2}\',but should be \",\" ,obj end or \']\'(array end)", json, originOffset, nextChar));
                }
            }
            return new JsonNode(json.Substring(originOffset, offset - originOffset + 1), json, originOffset);

        }


        private static string ParseName(string json, ref int offset)
        {
            int originOffset = offset;
            if (!json[offset].Equals('"'))
            {
                throw new SimpleJsonException(string.Format("can't parse name:json:{0},offset:{1} which char is \'{2}\',but should be \'\"\'(name start)", json, offset, json[offset]));
            }

            while (offset < json.Length)
            {
                if (json[offset].Equals('"') && json[offset + 1].Equals(':') && !json[offset - 1].Equals('\\'))
                {
                    break;
                }
                offset++;
            }

            char c = json[offset];

            if (!c.Equals('"'))
            {
                int error = offset;
                throw new SimpleJsonException(string.Format("can't parse name,json:{0},offset:{1}  which char is \'{2}\',but should be \'\"\'", json, error, json[error]));
            }

            return json.Substring(originOffset + 1, offset - originOffset - 1);
        }


        private static char NextUnBlankControlChar(string json, ref int offset)
        {
            offset++;
            if (offset > json.Length - 1)
            {
                throw new SimpleJsonException("can not get next char in json,because it's the end of json");
            }
            while (Char.IsControl(json[offset]) || Char.IsWhiteSpace(json[offset]))
            {
                offset++;
            }
            return json[offset];
        }

        private static char NextUnBlankControlCharWithNoRef(string json, int offset)
        {
            offset++;
            if (offset > json.Length - 1)
            {
                throw new SimpleJsonException("can not get next char in json,because it's the end of json");
            }
            while (Char.IsControl(json[offset]) || Char.IsWhiteSpace(json[offset]))
            {
                offset++;
            }
            return json[offset];
        }
        #endregion

        public override string ToString()
        {
            if (NodeType == NodeType.Value)
            {
                if (SuperNode == null)
                {
                    throw new SimpleJsonException(string.Format("error,value must have super node"));
                }
                if (SuperNode.NodeType == NodeType.Value)
                {
                    throw new SimpleJsonException(string.Format("error,value 's super node type can't be value"));
                }

                if (ValueType == ValueType.Null)
                {
                    return "null";
                }

                if (ValueType == ValueType.String)
                {
                    return string.Format("\"{0}\"", StringValue);
                }
                else
                {
                    return StringValue;
                }

            }

            var sb = new StringBuilder();
            if (NodeType == NodeType.Object)
            {
                sb.Append("{");

                bool first = true;

                foreach (KeyValuePair<string, JsonNode> keyValuePair in NodeDic)
                {
                    if (first)
                    {
                        sb.Append(string.Format("\"{0}\":{1}", keyValuePair.Key, keyValuePair.Value.ToString()));
                        first = false;
                    }
                    else
                    {
                        sb.Append(string.Format(",\"{0}\":{1}", keyValuePair.Key, keyValuePair.Value.ToString()));
                    }
                }


                sb.Append("}");
            }
            if (NodeType == NodeType.Array)
            {
                sb.Append("[");
                bool first = true;

                foreach (JsonNode jsonNode in Nodes)
                {
                    if (first)
                    {
                        sb.Append(string.Format("{0}", jsonNode.ToString()));
                        first = false;
                    }
                    else
                    {
                        sb.Append(string.Format(",{0}", jsonNode.ToString()));
                    }
                }

                sb.Append("]");
            }

            return sb.ToString();
        }




    }


}
