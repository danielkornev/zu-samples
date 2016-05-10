using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Analysis
{
	internal class WeightedListTruncator<T>
	{
		internal WeightedListTruncator()
		{
		}

		internal static int ComputeCut(IEnumerable<KeyValuePair<T, double>> list, int iLim)
		{
			if (!list.Any<KeyValuePair<T, double>>())
			{
				return 0;
			}
			int iFrom = Math.Min(list.Count<KeyValuePair<T, double>>(), Math.Max(1, iLim / 3));
			iLim = Math.Min(list.Count<KeyValuePair<T, double>>(), iLim);
			int res = iFrom;
			double largestDelta = 0;
			int i = 0;
			double scorePrev = list.First<KeyValuePair<T, double>>().Value;
			foreach (KeyValuePair<T, double> pair in list)
			{
				double delta = scorePrev - pair.Value;
				if (iFrom <= i && largestDelta < delta)
				{
					largestDelta = delta;
					res = i;
				}
				else if (iLim <= i)
				{
					break;
				}
				scorePrev = pair.Value;
				i++;
			}
			return res;
		}
	} // class
} // namespace