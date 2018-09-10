using UnityEngine;
using UnityEngine.InputNew;

// GENERATED FILE - DO NOT EDIT MANUALLY
namespace UnityEngine.InputNew
{
	public class MainToolInput : ActionMapInput {
		public MainToolInput (ActionMap actionMap) : base (actionMap) { }
		
		public AxisInputControl @touchXAxis { get { return (AxisInputControl)this[0]; } }
		public AxisInputControl @touchYAxis { get { return (AxisInputControl)this[1]; } }
	}
}
