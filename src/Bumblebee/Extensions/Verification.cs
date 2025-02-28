using System.Linq.Expressions;

using Bumblebee.Exceptions;
using Bumblebee.Interfaces;

using OpenQA.Selenium;

namespace Bumblebee.Extensions;

/// <summary>
/// Extension methods for verifying values.
/// </summary>
public static class Verification
{
	/// <summary>
	/// Verification method that allows for passing a predicate expression to evaluate some condition and a message to display if predicate is not true.
	/// </summary>
	/// <remarks>
	/// When throwing an error on verification, the system will add "Unable to verify " to anything that you pass as a message.  The recommendation is that you 
	/// write your verification strings starting with "that".  An example verification of "that string is empty." would return "Unable to verify that string is empty."
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <param name="verification"></param>
	/// <param name="predicate"></param>
	/// <returns></returns>
	public static T Verify<T>(this T obj, string verification, Predicate<T> predicate)
	{
		var message = $"Unable to verify.  {verification ?? String.Empty}".Trim();

		if (predicate(obj) == false)
		{
			throw CreateVerificationException(obj, message);
		}

		return obj;
	}

	/// <summary>
	/// Verification method that allows for passing a predicate expression to evaluate some condition.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj">The object.</param>
	/// <param name="predicateExpression">The expression.</param>
	/// <returns></returns>
	/// <remarks>
	/// If the predicate fails, the system will throw a verification exception with the message "Unable to verify custom verification."
	/// </remarks>
	public static T Verify<T>(this T obj, Expression<Predicate<T>> predicateExpression)
	{
		var predicate = predicateExpression.Compile();
		return obj.Verify($"Expected {predicateExpression.Body}", predicate);
	}

	/// <summary>
	/// Verification method that allows for passing an assertion from any assertion library.
	/// </summary>
	/// <remarks>
	/// The message that is thrown from the assertion library that you use will be captured in the VerificationException.
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="value"></param>
	/// <param name="assertion"></param>
	/// <returns></returns>
	public static T VerifyThat<T>(this T value, Action<T> assertion)
	{
		try
		{
			assertion(value);
			return value;
		}
		catch (Exception ex)
		{
			throw CreateVerificationException(value, $"Unable to verify.  {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Verifies whether or not the element is selected.
	/// </summary>
	/// <typeparam name="TSelectable">The type of the selectable.</typeparam>
	/// <param name="selectable">The selectable element.</param>
	/// <param name="selected">if set to <c>true</c> verifies whether element is selected and vice versa.</param>
	/// <returns></returns>
	/// <exception cref="VerificationException">Selection verification failed. Expected: {0}, Actual: {1}..FormatWith(selected, selectable.Selected)</exception>
	public static TSelectable VerifySelected<TSelectable>(this TSelectable selectable, bool selected) where TSelectable : ISelectable
	{
		if (selectable.Selected != selected)
		{
			throw CreateVerificationException(selectable, $"Selection verification failed. Expected: {selected}, Actual: {selectable.Selected}.");
		}

		return selectable;
	}

	public static THasText VerifyText<THasText>(this THasText hasText, string text) where THasText : IHasText
	{
		if (hasText.Text != text)
		{
			throw CreateVerificationException(hasText, $"Text verification failed. Expected:  {text}, Actual:  {hasText.Text}.");
		}

		return hasText;
	}

	public static THasText VerifyTextMismatch<THasText>(this THasText hasText, string text) where THasText : IHasText
	{
		if (hasText.Text == text)
		{
			throw CreateVerificationException(hasText, $"Text mismatch verification failed. Unexpected:  {text}, Actual:  {hasText.Text}.");
		}

		return hasText;
	}

	public static THasText VerifyTextContains<THasText>(this THasText hasText, string text) where THasText : IHasText
	{
		if (hasText.Text.Contains(text) == false)
		{
			throw CreateVerificationException(hasText, $"Expected \"{hasText.Text}\" to contain \"{text}\"");
		}

		return hasText;
	}

	public static TBlock VerifyPresence<TBlock>(this TBlock block, By @by) where TBlock : IBlock
	{
		return block.VerifyPresenceOf("element", @by);
	}

	public static TBlock VerifyAbsence<TBlock>(this TBlock block, By @by) where TBlock : IBlock
	{
		return block.VerifyAbsenceOf("element", @by);
	}

	public static TBlock VerifyPresenceOf<TBlock>(this TBlock block, string element, By @by) where TBlock : IBlock
	{
		if (block.FindElements(@by).Any() == false)
		{
			throw CreateVerificationException(block, $"Couldn't verify presence of {element} {@by}");
		}

		return block;
	}

	public static TBlock VerifyAbsenceOf<TBlock>(this TBlock block, string element, By @by) where TBlock : IBlock
	{
		if (block.FindElements(@by).Any())
		{
			throw CreateVerificationException(block, $"Couldn't verify absence of {element} {@by}");
		}

		return block;
	}

	public static TElement VerifyClasses<TElement>(this TElement block, IEnumerable<string> expectedClasses) where TElement : IElement
	{
		block.Tag.VerifyClasses(expectedClasses);

		return block;
	}

	public static TElement VerifyClasses<TElement>(this TElement block, params string[] expectedClasses) where TElement : IElement
	{
		return block.VerifyClasses((IEnumerable<string>) expectedClasses);
	}

	public static void VerifyClasses(this IWebElement element, IEnumerable<string> expectedClasses)
	{
		var classes = element.GetClasses();

		var missingClasses = expectedClasses.Where(expected => classes.Contains(expected) == false).ToList();

		if (missingClasses.Any())
		{
			throw CreateVerificationException(element, $"Block is missing the following expected classes: {String.Join(", ", missingClasses)}");
		}
	}

	public static void VerifyClasses(this IWebElement element, params string[] expectedClasses)
	{
		element.VerifyClasses(expectedClasses.AsEnumerable());
	}

	public static TBlock Store<TBlock, TData>(this TBlock block, out TData data, Func<TBlock, TData> exp)
	{
		data = exp(block);

		return block;
	}

	private static VerificationException CreateVerificationException(object item, string message, Exception innerException = null)
	{
		var hasSession = item as IHasSession;

		var session = hasSession?.Session;

		var settings = session?.Settings;

		if (settings != null && settings.CaptureScreenOnVerificationFailure)
		{
			var method = CallStack.GetFirstNonBumblebeeMethod();

			var methodName = method.GetFullName();

			var path = Path.Combine(Environment.CurrentDirectory, $"{methodName}.png");

			session.CaptureScreen(path);
		}

		return new VerificationException(message, innerException);
	}
}