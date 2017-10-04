using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FileManager.Extensions
{
    static class RichTextBoxExtensions
    {
        public static void AppendAbbreviationText(this RichTextBox box)
        {
            BrushConverter bc = new BrushConverter();
            foreach (var key in AbbreviationDictionary.Abbreviations.Keys)
            {
                TextRange tr = FindWordFromPosition(box.Document.ContentStart, key);
                try
                {
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                        bc.ConvertFromString("Green"));
                }
                catch { }
            }
        }

        static TextRange FindWordFromPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    int indexInRun = textRun.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                    if (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        return new TextRange(start, end);
                    }
                }

                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }

            return null;
        }
    }
}
