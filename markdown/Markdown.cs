using System;
using System.Text;
using System.Text.RegularExpressions;

public static class Markdown
{
    /// <summary>
    /// Putting text within element tags
    /// </summary>
    private static string WrapWithTag(string text, string tag) => $"<{tag}>{text}</{tag}>";

    /// <summary>
    /// Replace according delimiters with tags
    /// </summary>
    private static string ReplaceMarkdownWithHtml(string markdown, string delimiter, string tag)
    {
        var pattern = $"{delimiter}(.+?){delimiter}";
        var replacement = $"<{tag}>$1</{tag}>";
        return Regex.Replace(markdown, pattern, replacement);
    }

    /// <summary>
    /// Change bold text to strong html tag
    /// </summary>
    private static string ParseToBold(string markdown) => ReplaceMarkdownWithHtml(markdown, "__", "strong");

    /// <summary>
    /// Change italic text to em html tag
    /// </summary>
    private static string ParseToItalic(string markdown) => ReplaceMarkdownWithHtml(markdown, "_", "em");

    /// <summary>
    /// Converting header markdowns to header tags
    /// </summary>
    private static string ParseHeaders(string markdown)
    {
        int headerLevel = 0;

        while (markdown[headerLevel] == '#')
        {
            headerLevel++;
        }

        //Conditions for correct header tag
        if (headerLevel > 0 &&
            headerLevel <= 6 &&
            markdown.Length > headerLevel &&
            markdown[headerLevel] == ' ')
        {
            //Applying range syntax instead of Substring to prevent new string allocations
            string headerContent = markdown[(headerLevel + 1)..].Trim();
            return WrapWithTag(headerContent, $"h{headerLevel}");
        }

        return null;
    }

    /// <summary>
    /// Convert to an according html tag
    /// </summary>
    private static string ConvertMarkdownToHtml(string markdown, bool isListItem = false)
    {
        // Check if the line is a header
        var header = ParseHeaders(markdown);
        if (header != null) return header;

        // Parse the line for bold and italic text
        var htmlText = ParseToItalic(ParseToBold(markdown));

        return isListItem ? WrapWithTag(htmlText, "li") : WrapWithTag(htmlText, "p");
    }

    /// <summary>
    /// Parse markdown string to HTML
    /// </summary>
    public static string Parse(string markdown)
    {
        var lines = markdown.Split('\n');

        //Using StringBuilder for optimal string manipulation proccess
        var result = new StringBuilder();

        bool inList = false;

        foreach (var line in lines)
        {
            if (line.StartsWith("* "))
            {
                if (!inList)
                {
                    result.Append("<ul>");
                    inList = true;
                }

                // Ignore by removing the first "* "
                var listItem = ConvertMarkdownToHtml(line[2..], true);
                result.Append(listItem);
            }
            else
            {
                //Close the list before line or header convertion
                if (inList)
                {
                    result.Append("</ul>");
                    inList = false;
                }

                result.Append(ConvertMarkdownToHtml(line));
            }
        }

        // Close any unclosed list
        if (inList)
        {
            result.Append("</ul>");
        }

        return result.ToString();
    }
}