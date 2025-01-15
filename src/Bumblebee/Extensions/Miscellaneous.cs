namespace Bumblebee.Extensions;

public static class Miscellaneous
{
	public static void Each<T>(this IEnumerable<T> enumerable, Action<T> function)
	{
		foreach (var member in enumerable)
		{
			function(member);
		}
	}
}