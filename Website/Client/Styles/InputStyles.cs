namespace Website.Client.Styles
{
    public class InputStyles
    {
        internal static string GetInputDefaultStyle() => "w-full px-1 inline-block border-2 text-black rounded-lg hover:border-skyHover focus:ring-skyFocus focus:ring-0";
      
        internal static string GetInputCheckboxDefaultStyle() => "w-5 h-5 px-1 inline-block border-2 text-black rounded-lg hover:border-skyHover focus:ring-skyFocus focus:ring-0";

        internal static string GetBorderDefaultStyle(bool isInvalid) => isInvalid ? "border-red" : "border-black"; 
    }
}
