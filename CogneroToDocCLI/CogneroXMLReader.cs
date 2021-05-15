using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CogneroToDoc
{
    public class CogneroXMLReader
    {
        // Parse the XML file and create a CogneroTest object with it
        public static CogneroTest ParseFile(string filepath)
        {
            CogneroTest test = new CogneroTest();

            XElement testXML = XElement.Load(filepath);

            var questions = testXML.Element("Container").Element("Questions").Elements("Question");

            foreach (XElement question in questions)
            {
                CogneroQuestion ques = new CogneroQuestion();

                ques.Text = "";

                // From the limited data I've seen I only need to account for True/False type questions. Other question types work fine with current method.
                if (question.Attribute("Type").Value.Equals("True_False"))
                    ques.IsTrueFalse = true;

                ques.Text += removeXhtmlStuff(question.Element("XHtml").Value);

                var answerItems = question.Element("AnswerData").Element("Answers").Elements("AnswerItem");

                // Anything not T/F (i.e. multiple choice and "open-ended questions")
                if (!ques.IsTrueFalse)
                {
                    foreach (XElement answer in answerItems)
                    {
                        CogneroAnswer ans = new CogneroAnswer();

                        ans.Text = removeXhtmlStuff(answer.Element("XHtml").Value);

                        if (!ans.Text.Equals(""))
                        {
                            if (answer.Attribute("IsCorrectAnswer").Value.Equals("true"))
                                ans.IsSolution = true;

                            ques.Answers.Add(ans);
                        }
                    }
                }

                // T/F questions
                else
                {
                    // Need to add a "custom" answer to the question object to handle T/F type questions
                    CogneroAnswer ans = new CogneroAnswer();
                    ans.IsSolution = true;

                    // This element always holds either a 1 or 0 (ASCII character) and indicates if the answer is true/false respectively.
                    var answerXML = question.Element("AnswerData").Element("Answers").Element("AnswerItem").Element("AnswerPlainTextOrIndex");

                    if (answerXML.Value.Equals("1"))
                        ans.Text = "TRUE";
                    else
                        ans.Text = "FALSE";

                    ques.Answers.Add(ans);
                }

                test.Questions.Add(ques);
            }

            return test;
        }

        // Cognero XML test files store the relevant textual data with a bunch of HTML formatting things we don't need.
        // We could attempt to honor the HTML formatting as much as possible (font, size, bold, etc) but I have not implemented this and it might be difficult with the DocX library
        private static string removeXhtmlStuff(string str)
        {
            // Convert HTML codes into their closest ASCII representations
            str = Regex.Replace(str, @"&#8217;", "'");
            str = Regex.Replace(str, @"&#8212;", "-");
            str = Regex.Replace(str, @"&ndash;", "-");
            str = Regex.Replace(str, @"&rsquo;", "'");
            str = Regex.Replace(str, @"&ldquo;", "\"");
            str = Regex.Replace(str, @"&rdquo;", "\"");
            str = Regex.Replace(str, @"&amp;", "&");
            str = Regex.Replace(str, @"&nbsp;", " ");

            // Convert < > into their proper form (makes the next regex easier)
            str = Regex.Replace(str, @"&lt;", "<");
            str = Regex.Replace(str, @"&gt;", ">");

            // Remove anything between < and > (assumed to be HTML formatting)
            str = Regex.Replace(str, @"<(.|\n)*?>", string.Empty);

            // Remove anything in the format of &whatever; that doesn't have whitespace in it. Assumes all the other potential HTML character codes will be rare and non-essential.
            str = Regex.Replace(str, @"&[^\s];", string.Empty);

            return str;
        }
    }
}