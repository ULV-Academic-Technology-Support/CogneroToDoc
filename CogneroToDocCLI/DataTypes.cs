using System.Collections.Generic;

namespace CogneroToDoc
{
    // Represents the test parsed from the XML file in its entirety
    public class CogneroTest
    {
        public List<CogneroQuestion> Questions;

        public CogneroTest()
        {
            Questions = new List<CogneroQuestion>();
        }
    }

    // Represents a question and its possible answers
    public class CogneroQuestion
    {
        public string Text;
        public bool IsTrueFalse; // May need to make an enum for all possible question types in the future but this works fine for now.
        public List<CogneroAnswer> Answers;

        public CogneroQuestion()
        {
            Answers = new List<CogneroAnswer>();
            IsTrueFalse = false;
        }
    }

    // Represents a possible answer to the question
    public class CogneroAnswer
    {
        public bool IsSolution; // Defines whether or not the answer is the correct answer for the question.
        public string Text;
    }
}