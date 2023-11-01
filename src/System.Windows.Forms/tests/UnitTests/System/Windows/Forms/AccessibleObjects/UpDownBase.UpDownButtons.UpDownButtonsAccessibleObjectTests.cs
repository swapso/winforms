﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Windows.Forms.Automation;
using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;
using Windows.Win32.UI.Accessibility;
using static System.Windows.Forms.UpDownBase;
using static System.Windows.Forms.UpDownBase.UpDownButtons;

namespace System.Windows.Forms.Tests.AccessibleObjects;

public class UpDownBase_UpDownButtons_UpDownButtonsAccessibleObject
{
    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_Ctor_Default()
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        using UpDownButtons upDownButtons = new UpDownButtons(upDownBase);
        UpDownButtonsAccessibleObject accessibleObject = new UpDownButtonsAccessibleObject(upDownButtons);

        Assert.Equal(upDownButtons, accessibleObject.Owner);
        Assert.False(upDownBase.IsHandleCreated);
        Assert.False(upDownButtons.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_ControlType_IsSpinner_IfAccessibleRoleIsDefault()
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        // AccessibleRole is not set = Default

        object actual = upDownButtons.AccessibilityObject.GetPropertyValue(UIA_PROPERTY_ID.UIA_ControlTypePropertyId);

        Assert.Equal(UIA_CONTROLTYPE_ID.UIA_SpinnerControlTypeId, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_Role_IsSpinButton_ByDefault()
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        // AccessibleRole is not set = Default

        AccessibleRole actual = upDownButtons.AccessibilityObject.Role;

        Assert.Equal(AccessibleRole.SpinButton, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    public static IEnumerable<object[]> UpDownButtonsAccessibleObject_GetPropertyValue_ControlType_IsExpected_ForCustomRole_TestData()
    {
        Array roles = Enum.GetValues(typeof(AccessibleRole));

        foreach (AccessibleRole role in roles)
        {
            if (role == AccessibleRole.Default)
            {
                continue; // The test checks custom roles
            }

            yield return new object[] { role };
        }
    }

    [WinFormsTheory]
    [MemberData(nameof(UpDownButtonsAccessibleObject_GetPropertyValue_ControlType_IsExpected_ForCustomRole_TestData))]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_ControlType_IsExpected_ForCustomRole(AccessibleRole role)
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        upDownButtons.AccessibleRole = role;

        object actual = upDownButtons.AccessibilityObject.GetPropertyValue(UIA_PROPERTY_ID.UIA_ControlTypePropertyId);
        UIA_CONTROLTYPE_ID expected = AccessibleRoleControlTypeMap.GetControlType(role);

        Assert.Equal(expected, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_Name_ReturnsExpected()
    {
        const string name = "Test name";
        using SubUpDownBase upDownBase = new();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        upDownButtons.AccessibleName = name;

        object actual = upDownButtons.AccessibilityObject.GetPropertyValue(UIA_PROPERTY_ID.UIA_NamePropertyId);

        Assert.Equal(name, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_RuntimeId_ReturnsExpected()
    {
        using SubUpDownBase upDownBase = new();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;

        object actual = upDownButtons.AccessibilityObject.GetPropertyValue(UIA_PROPERTY_ID.UIA_RuntimeIdPropertyId);

        Assert.Equal(upDownButtons.AccessibilityObject.RuntimeId, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_BoundingRectangle_ReturnsExpected()
    {
        using SubUpDownBase upDownBase = new();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        object actual = upDownButtons.AccessibilityObject.GetPropertyValue(UIA_PROPERTY_ID.UIA_BoundingRectanglePropertyId);
        using SafeArrayScope<double> rectArray = UiaTextProvider.BoundingRectangleAsArray(upDownButtons.AccessibilityObject.BoundingRectangle);
        Assert.Equal(((VARIANT)rectArray).ToObject(), actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsTheory]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsExpandCollapsePatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsGridItemPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsGridPatternAvailablePropertyId))]
    [InlineData(true, ((int)UIA_PROPERTY_ID.UIA_IsLegacyIAccessiblePatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsMultipleViewPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsScrollItemPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsScrollPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsSelectionItemPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsSelectionPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsTableItemPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsTablePatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsTextPattern2AvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsTextPatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsTogglePatternAvailablePropertyId))]
    [InlineData(false, ((int)UIA_PROPERTY_ID.UIA_IsValuePatternAvailablePropertyId))]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_Pattern_ReturnsExpected(bool expected, int propertyId)
    {
        using TrackBar trackBar = new();
        using SubUpDownBase upDownBase = new();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        UpDownButtonsAccessibleObject accessibleObject = (UpDownButtonsAccessibleObject)upDownButtons.AccessibilityObject;

        Assert.Equal(expected, accessibleObject.GetPropertyValue((UIA_PROPERTY_ID)propertyId) ?? false);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsTheory]
    [InlineData((int)UIA_PROPERTY_ID.UIA_LegacyIAccessibleRolePropertyId, AccessibleRole.SpinButton)]
    [InlineData((int)UIA_PROPERTY_ID.UIA_LegacyIAccessibleStatePropertyId, AccessibleStates.None)]
    [InlineData((int)UIA_PROPERTY_ID.UIA_ValueValuePropertyId, null)]
    public void UpDownButtonsAccessibleObject_GetPropertyValue_ReturnsExpected(int property, object expected)
    {
        using TrackBar trackBar = new();
        using SubUpDownBase upDownBase = new();
        UpDownButtons upDownButtons = upDownBase.UpDownButtonsInternal;
        UpDownButtonsAccessibleObject accessibleObject = (UpDownButtonsAccessibleObject)upDownButtons.AccessibilityObject;
        object actual = accessibleObject.GetPropertyValue((UIA_PROPERTY_ID)property);

        Assert.Equal(expected, actual);
        Assert.False(upDownBase.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_FragmentNavigate_Parent_ReturnsNull()
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        using UpDownButtons upDownButtons = new UpDownButtons(upDownBase);
        UpDownButtonsAccessibleObject accessibleObject = new UpDownButtonsAccessibleObject(upDownButtons);

        Assert.Null(accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_Parent));
        Assert.False(upDownBase.IsHandleCreated);
        Assert.False(upDownButtons.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_FragmentNavigate_Sibling_ReturnsNull()
    {
        using UpDownBase upDownBase = new SubUpDownBase();
        using UpDownButtons upDownButtons = new UpDownButtons(upDownBase);
        UpDownButtonsAccessibleObject accessibleObject = new UpDownButtonsAccessibleObject(upDownButtons);

        Assert.Null(accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_NextSibling));
        Assert.Null(accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_PreviousSibling));
        Assert.False(upDownBase.IsHandleCreated);
        Assert.False(upDownButtons.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_FragmentNavigate_Child_ReturnsExpected_InNumericUpDown()
    {
        using NumericUpDown numericUpDown = new();
        UpDownButtonsAccessibleObject accessibleObject = (UpDownButtonsAccessibleObject)numericUpDown.UpDownButtonsInternal.AccessibilityObject;

        // UpButton has 0 childId, DownButton has 1 childId
        AccessibleObject upButton = accessibleObject.GetChild(0);
        AccessibleObject downButton = accessibleObject.GetChild(1);

        Assert.Equal(upButton, accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_FirstChild));
        Assert.Equal(downButton, accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_LastChild));
        Assert.False(numericUpDown.IsHandleCreated);
    }

    [WinFormsFact]
    public void UpDownButtonsAccessibleObject_FragmentNavigate_Child_ReturnsExpected_InDomainUpDown()
    {
        using DomainUpDown domainUpDown = new();
        UpDownButtonsAccessibleObject accessibleObject = (UpDownButtonsAccessibleObject)domainUpDown.UpDownButtonsInternal.AccessibilityObject;

        // UpButton has 0 childId, DownButton has 1 childId
        AccessibleObject upButton = accessibleObject.GetChild(0);
        AccessibleObject downButton = accessibleObject.GetChild(1);

        Assert.Equal(upButton, accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_FirstChild));
        Assert.Equal(downButton, accessibleObject.FragmentNavigate(NavigateDirection.NavigateDirection_LastChild));
        Assert.False(domainUpDown.IsHandleCreated);
    }

    private class SubUpDownBase : UpDownBase
    {
        protected override void UpdateEditText() => throw new NotImplementedException();

        public override void UpButton() => throw new NotImplementedException();

        public override void DownButton() => throw new NotImplementedException();
    }
}
