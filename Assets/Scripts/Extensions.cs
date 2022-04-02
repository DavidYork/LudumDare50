using Ink.Runtime;
using System;

public static class Extensions {
    public static string Sanitize(this string storyBit) {
        storyBit = storyBit.Replace("<cmd>", "<color=white>");
        storyBit = storyBit.Replace("</cmd>", "</color>");
        storyBit = storyBit.Replace("<br/>", "");
        return storyBit;
    }

	public static TEnum ToEnum<TEnum>(this InkList src) where TEnum: struct, System.Enum {
		var values = src.Values;
		var enumerator = values.GetEnumerator();
		enumerator.MoveNext();
		var val = enumerator.Current;

		TEnum enumVal = (TEnum)Enum.ToObject(typeof(TEnum) , val - 1);
		return enumVal;
	}

	public static void SetVariableToListItem<TEnum>(this Story story, string name, TEnum newValue)
		where TEnum: struct, Enum
	{
		var fullName = $"{newValue.GetType()}";
		var enumName = fullName.Substring(fullName.LastIndexOf('+') + 1);
		var newList = new Ink.Runtime.InkList(enumName, story);
		newList.AddItem($"{newValue}");
		story.variablesState[name] = newList;
	}
}