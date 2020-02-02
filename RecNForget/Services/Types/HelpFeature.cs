using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services.Types
{
	/// <summary>
	/// a HelpFeature represents a feature, one possible action or configuration possibility inside the program
	/// the sum of all such HelpFeatures make up the application's complete functionality
	/// </summary>
	public class HelpFeature
	{
		/// <summary>
		/// Unique Feature Identifier (every feature gets its own clas)
		/// </summary>
		public string Id
		{
			get
			{
				return this.GetType().Name;
			}
		}

		/// <summary>
		/// whether this is actually a feature/ change/ bugfix/ etc
		/// </summary>
		public HelpFeatureClass FeatureClass { get; set; } = HelpFeatureClass.NewFeature;

		/// <summary>
		/// Version when feature was introduced
		/// </summary>
		public Version MinVersion { get; set; }

		/// <summary>
		/// last supported version for this feature
		/// (should be rarely used, but sometimes a feature just gets removed
		/// </summary>
		public Version MaxVersion { get; set; } = null;

		/// <summary>
		/// VERY short one line teaser (only slightly longer than FeatureClass)
		/// </summary>
		public string Title { get; set; } = "FeatureTitle";

		/// <summary>
		/// USE WITH CAUTION
		/// (these should stay rather "unique" throughout all versions)
		/// for sorting purposes the lower the priority number the lower the index in target collection
		/// </summary>
		public int Priority { get; set; } = int.MaxValue;

		/// <summary>
		/// each element represents a (EXISTING) help feature id (inside the same list) to refereence similar articles or topics
		/// </summary>
		public List<string> SeeAlsoIds { get; set; } = null;

		/// <summary>
		/// for generating in application help views AND for generating feature list md files for github
		/// Detailed help contents explaining
		/// how and where the feature works
		/// what it requires
		/// and how it can be changed
		/// a single line might be plain simple text or an image (just actual image pixel size , no resizing for now)
		/// </summary>
		public List<HelpFeatureDetailLine> HelpLines { get; set; } = null;

		/// <summary>
		/// shorter help lines controls can subscribe to and show when hovered over by mouse
		/// </summary>
		public List<HelpFeatureDetailLine> HoverHelpLines { get; set; } = null;

		public static List<HelpFeature> All
		{
			get
			{
				var featureList = new List<HelpFeature>();
				var thisAssembly = System.Reflection.Assembly.GetExecutingAssembly();

				// very cheap shot to include everything under RecNForget.Help.Features namespace, but not precise
				// ToDo, cant we use nameof somehow? or make this no magic string?
				var featureClasses = thisAssembly.GetTypes().Where(t => t.Namespace.StartsWith("RecNForget.Help.Features") && t.BaseType == typeof(HelpFeature));

				foreach (var feature in featureClasses.OrderBy(f => f.Name))
				{
					featureList.Add((HelpFeature)Activator.CreateInstance(feature));
				}

				return featureList;
			}

		}

		public string HelpLinesAsString()
		{
			StringBuilder detailsHelpBuilder = new StringBuilder();
			for (int i = 0; i < this.HelpLines.Count; i++)
			{

				if (i == (this.HelpLines.Count - 1))
				{
					detailsHelpBuilder.Append(this.HelpLines[i].Content);
				}
				else
				{
					detailsHelpBuilder.AppendLine(this.HelpLines[i].Content);
				}
			}

			return detailsHelpBuilder.ToString();
		}
	}
}
