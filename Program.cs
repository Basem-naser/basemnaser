using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Removestrar
{
	class Program
	{
		public static string RemoveStars(string input)
		{
			int startIndex = input.IndexOf('*', 0);
			if ( startIndex == -1 )
				return input;
			else
			{
				
				string leftSide = startIndex < 1 ? "" : input.Substring(0, startIndex - 1);
				string rightSide = startIndex == input.Length-1 ? "" : input.Substring( startIndex + 1);
				return RemoveStars(leftSide + rightSide);
			}
		}

		public static string Decode (string text)
		{
			if ( text == null || text.Length == 0 ) return text;
			if ( char.IsDigit(text[0]))
			{
				int startIndex = text.IndexOf('[', 0);
				int closeIndex = startIndex + 1;
				int numberOfOpening = 1;
				while ( closeIndex  < text.Length && numberOfOpening >0 )
				{
					if ( text[closeIndex] =='[')
					{
						numberOfOpening++;
					}
					else
						 if ( text[closeIndex] == ']' )
						{
							numberOfOpening--;
						}
					closeIndex++;
				}
				StringBuilder repeatedDecoded = new StringBuilder();
				string decoded = Decode(text.Substring(startIndex + 1, closeIndex - 2 - startIndex));
				for ( 
					int i = 0 ; i < int.Parse(text.Substring(0, startIndex)) ; i++ )
				{
					repeatedDecoded.Append(decoded);
				}
				return repeatedDecoded.ToString() + Decode(text.Substring(closeIndex));
			}
			else
			{
				int startDigit = 1;
				while ( startDigit < text.Length && !char.IsDigit(text[startDigit]) )
					startDigit++;
				return text.Substring(0, startDigit) + Decode(text.Substring(startDigit,text.Length - startDigit));
			}
		}

		private static int CalculateOperation(string leftOperand, string rightOperand, string operation)
		{
			switch ( operation )
			{
				case "+": return int.Parse(leftOperand) + int.Parse(rightOperand);
				case "-":
					return int.Parse(leftOperand) - int.Parse(rightOperand);
				case "*":
					return int.Parse(leftOperand) * int.Parse(rightOperand);
				case "/":
					return int.Parse(leftOperand) / int.Parse(rightOperand);
			}
			throw new Exception("not legal operation");
		}
		public static int EvaluatePolish(string polish)
		{
			string[] tokens = polish.Split(",");
			
			int opIndex = 0;
			while ( tokens[opIndex] != "*" && tokens[opIndex] != "+" && tokens[opIndex] != "-" && tokens[opIndex] != "/" )
				opIndex++;
			
			int calculated = CalculateOperation(tokens[opIndex - 2], tokens[opIndex - 1], tokens[opIndex]);
			if ( opIndex == 2 && tokens.Length == 3 )
				return calculated;
			else
			{
				StringBuilder newPolish = new StringBuilder();
				if ( opIndex > 2 )
				{
					string[] leftTokens = new string[opIndex - 2];
					Array.Copy(tokens, leftTokens, opIndex - 2);
					newPolish.Append(string.Join(',', leftTokens));
					newPolish.Append(",");
				}
				newPolish.Append(calculated.ToString());
				if ( opIndex < tokens.Length )
				{
					newPolish.Append(",");
					string[] rightTokens = new string[tokens.Length - opIndex - 1];
					Array.Copy(tokens, opIndex + 1, rightTokens, 0, tokens.Length - opIndex - 1);
					newPolish.Append(string.Join(',', rightTokens));
				}
				return EvaluatePolish(newPolish.ToString());
			}

			
		}
		public static string RemoveTagString(string polish)
		{
			return string.Join(",", polish.Split(",").Select(word => word.Substring(1, word.Length - 2)).ToArray());
		}

		public class ListNode
		{
			public int val;
			public ListNode next;

			public ListNode(int val = 0, ListNode next = null)
			{
				this.val = val;
				this.next = next;
			}
		}
		public static ListNode Merge(ListNode a, ListNode b)
		{
			if ( a == null )
				return b;
			if ( b == null )
				return a;

			ListNode result;

			if ( a.val < b.val )
			{
				result = a;
				result.next = Merge(a.next, b);
			}
			else
			{
				result = b;
				result.next = Merge(a, b.next);
			}

			return result;
		}


		public static ListNode RemoveNthFromEnd(ListNode head, int n)
		{
			ListNode previous = head;
			ListNode current = head;
			for ( int i = 0 ; current != null && i < n ; i++ )
			{
				current = current.next;
			}
			while ( current != null && current.next != null)
			{
				current = current.next;
				previous = previous.next;
			}
			if (previous == head)
			{
				return head.next;
			}
			else
			{
				if (previous.next.next == null)
				{
					previous.next = null;
				}				
				else
				{
					previous.next = previous.next.next;
				}
				return head;
			}
			

		}

		// Helper: Create linked list from array
		public static ListNode CreateList(int[] values)
		{
			if ( values.Length == 0 )
				return null;

			ListNode head = new ListNode(values[0]);
			ListNode current = head;

			for ( int i = 1 ; i < values.Length ; i++ )
			{
				current.next = new ListNode(values[i]);
				current = current.next;
			}

			return head;
		}

		public static ListNode RotateList(ListNode head, int n)
		{
			ListNode newLastNode = head;
			ListNode lastNode = head;
			for ( int i = 0 ;  i < n ; i++ )
			{
				if ( lastNode.next == null )
					lastNode = head;
				else 
					lastNode = lastNode.next;
			}
			if ( newLastNode == lastNode )
				return head;

			while ( lastNode.next != null )
			{
				lastNode = lastNode.next;
				newLastNode = newLastNode.next;
			}
			lastNode.next = head;
			head = newLastNode.next;
			newLastNode.next = null;
			return head;			
		}

		public static ListNode RemoveDulpicatesFromSortedList(ListNode head)
		{			
			ListNode current = head;
			ListNode previousCurrent = head;
			while ( current != null)
			{
				ListNode nextNode = current.next;
				while ( nextNode != null && current.val == nextNode.val )
					nextNode = nextNode.next;
				if (nextNode == current.next) // no delete
				{
					previousCurrent = current;
				}
				else
				{
					if ( previousCurrent == head )
					{
						head = nextNode;
						previousCurrent = nextNode;
					}
					else
					{
						previousCurrent.next = nextNode;
					}
				}				
				current = nextNode;
			}			
			return head;
		}
		public static ListNode PartitionList(ListNode head,int val)
		{
			ListNode current = head;
			ListNode previousGreatNode = head;
			while ( current != null )
			{
				ListNode nextNode = current.next;
				while ( current.val < val )
				{
					previousGreatNode = current;
					current = current.next;
				}
				ListNode prevLessNode = current;
				while (current!= null)
				{					
					while ( current != null && current.val >= val)
					{
						prevLessNode = current;
						current = current.next;
					}
					if ( current != null )
					{
						ListNode nextCurrent = current.next;
						if ( head.val>=val )
						{
							current.next = previousGreatNode;
							previousGreatNode.next = nextCurrent;
							head = current;
							prevLessNode = previousGreatNode;
							previousGreatNode = current;
						}
						else
						{
							prevLessNode.next = current.next;
							current.next = previousGreatNode.next;
							previousGreatNode.next = current;
							previousGreatNode = current;
						}
						current = nextCurrent;
					}
				}				
			}
			return head;
		}

		public static ListNode ReverseList(ListNode head)
		{
			ListNode prevNode = null;
			ListNode current = head;
			
			
			while ( current != null )
			{
				ListNode nextcurrent = current.next;
				current.next = prevNode;
				prevNode = current;
				current = nextcurrent;
			}			
			return prevNode;
		}

		public static ListNode ReverseList(ListNode head,int start, int last)
		{
			ListNode prevNode = null;
			ListNode current = head;
			while (current.val != start)
			{
				prevNode = current;
				current = current.next;
			}
			ListNode innerPrevNode = null;
			ListNode nextcurrent = null;
			bool stop = false;
			while ( !stop )
			{
				stop = current.val == last;
				nextcurrent = current.next;
				current.next = innerPrevNode;
				innerPrevNode = current;
				if (!stop) current = nextcurrent;
			}
			if ( prevNode  == null)
			{
				head.next = nextcurrent;
				head = current;
			}
			else
			{
				prevNode.next.next = nextcurrent;
				prevNode.next = current;
			}
			return head;
		}


		public static ListNode reverseKGroup(ListNode head, int k)
		{
			ListNode prevHead = null;
			ListNode current = head;
			ListNode innerPrevNode = null;
			ListNode nextcurrent = null;
			ListNode startHeadcurrent = head; 
			bool stop = false;
			int groupCount = 0;
			while ( !stop)
			{
				groupCount++;				
				stop = current.next == null;
				nextcurrent = current.next;
				current.next = innerPrevNode;
				innerPrevNode = current;
				if ( groupCount == k || stop )
				{
					groupCount = 0;
					innerPrevNode = null;			
					if ( startHeadcurrent == head )
					{						
						head = current;						
					}
					if (prevHead != null)
					{
						prevHead.next = current;
					}
					prevHead = startHeadcurrent;
					startHeadcurrent.next = nextcurrent;
					startHeadcurrent = nextcurrent;					
				}
				if ( !stop )
					current = nextcurrent;
				
			}
			
			return head;
		}

		public static int getMiddle(ListNode head)
		{
			
			ListNode middle = head;
			ListNode fast = head.next;
			while ( fast != null)
			{				
				middle = middle.next;
				if ( fast.next == null )
				{
					fast = fast.next;
				}
				else
				{
					fast = fast.next.next;
				}
			}
			
			return middle.val;
		}

		public static bool HasListCycle(ListNode head)
		{
			if ( head == null )
				return false;
				ListNode current = head;
			ListNode fastNode = current.next;
			while ( current != null && fastNode!=null && current!=fastNode)
			{
				current = current.next;
				fastNode = fastNode.next;
				if (fastNode!=null)
					fastNode = fastNode.next;
			}
			return current == fastNode;
		}
		public static void BuildListCycle(ListNode head, int toWhere, int fromWhere)
		{
			ListNode toWhereNode = null, fromWhereNode = null;

			ListNode current = head;
			while ( current != null && fromWhereNode == null)
			{
				if (current.val == toWhere )
				{
					toWhereNode = current;
				}
				if ( current.val == fromWhere )
				{
					fromWhereNode = current;
				}
				current = current.next;			
			}
			fromWhereNode.next = toWhereNode;
		}

		public static void PrintList(ListNode head)
		{
			while ( head != null )
			{
				Console.Write(head.val + " -> ");
				head = head.next;
			}
			Console.WriteLine("null");
		}

		public class TreeNode
		{
			public int val;
			public TreeNode left;
			public TreeNode right;
			public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
			{
				this.val = val;
				this.left = left;
				this.right = right;
			}
		}

		public class Solution
		{
			public IList<int> LeftView(TreeNode root)
			{
				List<int> result = new List<int>();
				if ( root == null )
					return result;

				Queue<TreeNode> queue = new Queue<TreeNode>();
				queue.Enqueue(root);

				while ( queue.Count > 0 )
				{
					int levelSize = queue.Count;

					for ( int i = 0 ; i < levelSize ; i++ )
					{
						TreeNode current = queue.Dequeue();

						if ( i == 0 )
						{
							result.Add(current.val);
						}

						if ( current.left != null )
							queue.Enqueue(current.left);
						if ( current.right != null )
							queue.Enqueue(current.right);
					}
				}

				return result;
			}
		}
			static void Main(string[] args)
		{
			//string text = "erase*****";
			//Console.WriteLine("text="+ text + ",  RemoveStars=" + RemoveStars(text));
			//var input = "3[a2[c]]";
			//Console.WriteLine("input=" + input + ",  Decode=" + Decode(input));
			//string polish = "\"10\",\"6\",\"9\",\"3\",\"+\",\"-11\",\"*\",\"/\",\"*\",\"17\",\"+\",\"5\",\"+\"";//"\"4\",\"13\",\"5\",\"/\",\"+\"";//"\"2\",\"1\",\"+\",\"3\",\"*\"";
			//Console.WriteLine("polish=" + polish + ",  EvaluatePolish=" + EvaluatePolish(RemoveTagString(polish)));

			//ListNode head = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
			////ListNode head = new ListNode(1);
			////ListNode head = new ListNode(1, new ListNode(2));

			//Console.WriteLine("Original List:");
			//PrintList(head);


			//ListNode head2 = RemoveNthFromEnd(head,1);

			//Console.WriteLine("\nList after removing 2nd node from the end:");
			//PrintList(head2);
			//int n = 2;
			////ListNode head3 = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
			//ListNode head3 = new ListNode(0, new ListNode(1, new ListNode(2)));
			//Console.WriteLine("\n The List before rotating" + n.ToString() + " places :\n");
			//PrintList(head3);

			//Console.WriteLine("\nRotateList " + n.ToString() + " places :\n");
			//PrintList(RotateList(head3, n));

			////ListNode sorted = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(3, new ListNode(4, new ListNode(4, new ListNode(5)))))));
			//ListNode sorted = new ListNode(1, new ListNode(1, new ListNode(1, new ListNode(2, new ListNode(3)))));
			//Console.WriteLine("\nRemoveDulpicatesFromSortedList ");
			//PrintList(sorted);
			//Console.WriteLine("\nAfter removing ");
			//PrintList(RemoveDulpicatesFromSortedList(sorted));
			////ListNode partition = new ListNode(1, new ListNode(4, new ListNode(3, new ListNode(2, new ListNode(5, new ListNode(2))))));
			//ListNode partition = new ListNode(2, new ListNode(1 ));
			//Console.WriteLine("\npartitionList ");
			//PrintList(partition);
			//Console.WriteLine("\nAfter partition ");
			//PrintList(PartitionList(partition,2));
			ListNode listToReverse = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
			Console.WriteLine("\nBefore Rverese ");
			PrintList(listToReverse);
			Console.WriteLine("\nAfter Rverese ");
			PrintList(ReverseList(listToReverse));

			ListNode listToReverse2 = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
			Console.WriteLine("\nBefore Rverese2 ");
			PrintList(listToReverse2);
			Console.WriteLine("\nAfter Rverese2 ");
			PrintList(ReverseList(listToReverse2, 1, 4));
			////ListNode cycleList = new ListNode(1, new ListNode(2, new ListNode(3, new ListNode(4, new ListNode(5)))));
			////BuildListCycle(cycleList, 2, 4);
			////ListNode cycleList = new ListNode(1, new ListNode(2));
			////BuildListCycle(cycleList, 1, 2);
			//ListNode cycleList = new ListNode(1);

			//if ( HasListCycle (cycleList))
			//	Console.WriteLine("\nThere is a cycle in the linked list ");
			//else
			//	Console.WriteLine("\nThere is no cycle in the linked list ");


			TreeNode root = new TreeNode(1,
				 new TreeNode(2),
				 new TreeNode(3,
					 new TreeNode(4, null, new TreeNode(5))));

			Solution sol = new Solution();
			IList<int> leftView = sol.LeftView(root);

			Console.WriteLine("Left view of the binary tree:");
			foreach ( int val in leftView )
			{
				Console.Write(val + " ");
			}

			string text = "Hello @John, please meet @JaneDoe and @Sam_123!";
			string pattern = @"@(\w+)";

			MatchCollection matches = Regex.Matches(text, pattern);

			foreach ( Match match in matches )
			{
				Console.WriteLine($"Found mention: {match.Value}");
				Console.WriteLine($"Captured username: {match.Groups[1].Value}");
			}


			int[] list1 = { 1, 3, 5, 7 };
			int[] list2 = { 2, 4, 6, 8 };

			ListNode l1 = CreateList(list1);
			ListNode l2 = CreateList(list2);

			ListNode merged = Merge(l1, l2);

			PrintList(merged); // Output: 1 2 3 4 5 6 7 8
			ListNode l3 = CreateList(new int[] { 1, 2, 3, 4, 5 });
			PrintList(l3);
			Console.WriteLine(string.Format("middle ={0}", getMiddle(l3)));

			Console.WriteLine(string.Format("middle ={0}", getMiddle(CreateList(new int[] { 2, 4, 6, 7, 5, 1 }))));
			ListNode l4 = CreateList(new int[] { 1, 2, 2, 4, 5, 6, 7, 8 ,9 , 10});
			PrintList(l4);
			Console.WriteLine("Reversion group ------");
			PrintList(reverseKGroup(l4, 4));
		}
	}
}
