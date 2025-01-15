using System.Linq.Expressions;

using OpenQA.Selenium;

namespace Bumblebee.Specifications;

internal class Specification : ISpecification
{
	public By Attribute(string attributeToFind, string attributeValueToFind) =>
		By.CssSelector($@"[{attributeToFind}='{attributeValueToFind}']");

	public By Attribute(string attributeToFind, string attributeValueToFind, TimeSpan timeout) =>
		Attribute(attributeToFind, attributeValueToFind).WaitingUntil(timeout);

	public By Id(string idToFind) =>
		By.Id(idToFind);

	public By Id(string idToFind, TimeSpan timeout) =>
		Id(idToFind).WaitingUntil(timeout);

	public By ClassName(string classNameToFind) =>
		By.ClassName(classNameToFind);

	public By ClassName(string classNameToFind, TimeSpan timeout) =>
		By.ClassName(classNameToFind).WaitingUntil(timeout);

	public By CssSelector(string cssSelectorToFind) =>
		By.CssSelector(cssSelectorToFind);

	public By CssSelector(string cssSelectorToFind, TimeSpan timeout) =>
		By.CssSelector(cssSelectorToFind).WaitingUntil(timeout);

	public By Function(Expression<Func<ISearchContext, IWebElement>> findElementMethod) =>
		new ByFunctionWithSingleOutput(findElementMethod);

	public By Function(Expression<Func<ISearchContext, IWebElement>> findElementMethod, TimeSpan timeout) =>
		Function(findElementMethod).WaitingUntil(timeout);

	public By Function(Expression<Func<ISearchContext, IEnumerable<IWebElement>>> findElementsMethod) =>
		new ByFunctionWithListOutput(findElementsMethod);

	public By Function(Expression<Func<ISearchContext, IEnumerable<IWebElement>>> findElementsMethod,
		TimeSpan timeout) =>
		Function(findElementsMethod).WaitingUntil(timeout);

	public By LinkText(string linkTextToFind) =>
		By.LinkText(linkTextToFind);

	public By LinkText(string linkTextToFind, TimeSpan timeout) =>
		By.LinkText(linkTextToFind).WaitingUntil(timeout);

	public By Name(string nameToFind) =>
		By.Name(nameToFind);

	public By Name(string nameToFind, TimeSpan timeout) =>
		By.Name(nameToFind).WaitingUntil(timeout);

	public By Ordinal(By @by, int ordinal) => 
		new ByOrdinal(by, ordinal);

	public By Ordinal(By @by, int ordinal, TimeSpan timeout) => 
		new ByOrdinal(by, ordinal).WaitingUntil(timeout);

	public By PartialLinkText(string partialLinkTextToFind) =>
		By.PartialLinkText(partialLinkTextToFind);

	public By PartialLinkText(string partialLinkTextToFind, TimeSpan timeout) =>
		By.PartialLinkText(partialLinkTextToFind).WaitingUntil(timeout);

	public By TagName(string tagNameToFind) => By.TagName(tagNameToFind);

	public By TagName(string tagNameToFind, TimeSpan timeout) => 
		By.TagName(tagNameToFind).WaitingUntil(timeout);

	public By XPath(string xPathToFind) => 
		By.XPath(xPathToFind);

	public By XPath(string xPathToFind, TimeSpan timeout) => 
		By.XPath(xPathToFind).WaitingUntil(timeout);
}