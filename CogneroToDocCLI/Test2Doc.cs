using System;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace CogneroToDoc
{
	public class Test2Doc
	{
		public static void CreateDocsFromTest(CogneroTest test, string filename)
		{
			CreateQuestionsDoc(test, filename);
			CreateSolutionsDoc(test, filename);
		}

		// Questions Document contains the questions and possible answers but NOT the solutions.
		public static void CreateQuestionsDoc(CogneroTest test, string filename)
		{
			var doc = DocX.Create(filename + ".docx");

			int quesNum = 1;
			foreach (CogneroQuestion ques in test.Questions)
			{
				doc.InsertParagraph(quesNum + ". " + (ques.IsTrueFalse ? "(T/F) " : "") + ques.Text);

				if (!ques.IsTrueFalse)
				{
					Xceed.Document.NET.List list = doc.AddList();

					foreach (CogneroAnswer ans in ques.Answers)
					{
						doc.AddListItem(list, ans.Text);
					}

					doc.InsertList(list);
				}

				doc.InsertParagraph();
				quesNum++;
			}

			doc.Save();
		}

		// This document contains the questions and only correct answers.
		public static void CreateSolutionsDoc(CogneroTest test, string filename)
		{
			Formatting solutionFormatting = new Formatting();
			solutionFormatting.Italic = true;

			var doc = DocX.Create(filename + " SOLUTIONS.docx");

			int quesNum = 1;
			foreach (CogneroQuestion ques in test.Questions)
			{
				doc.InsertParagraph(quesNum + ". " + (ques.IsTrueFalse ? "(T/F) " : "") + ques.Text);

				foreach (CogneroAnswer ans in ques.Answers)
				{
					if (ans.IsSolution)
						doc.InsertParagraph("SOLUTION: " + ans.Text, false, solutionFormatting);
				}

				doc.InsertParagraph();
				quesNum++;
			}

			doc.Save();
		}
	}
}