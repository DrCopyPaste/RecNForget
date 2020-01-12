using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecNForget.Services.Types
{
	public enum HelpFeatureClass
	{
		// obvious bugfix should be obvious
		BugFix,

		// anytime a new configuration / something visibly/controllably new / new behavior is added
		NewFeature,

		// anytime some existing look and feel / behavior / default settings change
		FeatureChange,

		// rare should almost never be used, maybe for some major reworks that you want to communicate but dont get really noticed by the user?
		Information
	}
}
