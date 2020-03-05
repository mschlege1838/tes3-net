using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TES3.Util
{
    public class HTMLTagWriter : IDisposable
    {


        static readonly Regex ReservedCharacters = new Regex("[\"&'<>]");
        static readonly IDictionary<string, string> ReservedReplacements = new Dictionary<string, string>
        {
            ["\""] = "&quot;",
            ["&"] = "&amp;",
            ["'"] = "&apos;",
            ["<"] = "&lt",
            [">"] = "&gt;"
        };

        static readonly ISet<string> RequiresBodyTags = new HashSet<string> { "script", "button", "select" };


        readonly TextWriter writer;

        bool inAttributes = false;
        readonly Stack<string> tagStack = new Stack<string>();

        public HTMLTagWriter(TextWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException("writer");
        }

        public void WriteDoctype()
        {
            writer.WriteLine("<!DOCTYPE html>");
        }

        public void WriteLine(string value)
        {
            writer.WriteLine(value);
        }

        public void Include(string fname)
        {
            if (inAttributes)
            {
                writer.Write(">");
                inAttributes = false;
            }

            var file = new FileInfo(fname);
            if (!file.Exists)
            {
                throw new ArgumentException($"{fname} does not exist.", "fname");
            }

            var buf = new char[128];
            var content = new StringBuilder();
            using (var stream = file.OpenText())
            {
                int count;
                while ((count = stream.Read(buf, 0, buf.Length)) != 0)
                {
                    content.Append(buf, 0, count);
                }
            }

            writer.WriteLine();
            writer.WriteLine(content);
        }

        public HTMLTagWriter WriteDirect(string content)
        {
            if (inAttributes)
            {
                writer.Write(">");
                inAttributes = false;
            }
            writer.WriteLine();
            writer.WriteLine(content);
            return this;
        }

        public HTMLTagWriter StartTag(string name)
        {
            if (inAttributes)
            {
                writer.Write(">");
            }
            inAttributes = true;

            var escapedName = XmlEscape(name);
            tagStack.Push(escapedName);

            writer.Write("<");
            writer.Write(escapedName);

            return this;
        }

        public HTMLTagWriter Attribute(string name, object value = null)
        {
            if (!inAttributes)
            {
                throw new InvalidOperationException();
            }

            writer.Write(" ");
            writer.Write(XmlEscape(name));
            if (value != null)
            {
                writer.Write("=\"");
                writer.Write(XmlEscape(value));
                writer.Write("\"");
            }

            return this;
        }

        public HTMLTagWriter Content(object content)
        {
            if (inAttributes)
            {
                writer.Write(">");
                inAttributes = false;
            }

            writer.Write(XmlEscape(content));
            return this;
        }

        public HTMLTagWriter CloseTag()
        {
            if (tagStack.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var name = tagStack.Pop();
            if (inAttributes)
            {
                if (RequiresBodyTags.Contains(name))
                {
                    writer.Write($"></{name}>");
                }
                else
                {
                    writer.Write(" />");
                }
            }
            else
            {
                writer.Write($"</{name}>");
            }

            inAttributes = false;
            return this;
        }

        public void Dispose()
        {
            writer.Dispose();
        }

        static string XmlEscape(object o)
        {
            return o == null ? "" : XmlEscape(o.ToString());
        }

        static string XmlEscape(string s)
        {
            var match = ReservedCharacters.Match(s);
            if (!match.Success)
            {
                return s;
            }

            var index = 0;
            var result = new StringBuilder();
            do
            {
                result.Append(s.Substring(index, match.Index - index));
                result.Append(ReservedReplacements[match.Value]);
                index = match.Index + match.Length;
            } while ((match = match.NextMatch()).Success);

            if (index < s.Length)
            {
                result.Append(s.Substring(index));
            }

            return result.ToString();
        }


    }
}
