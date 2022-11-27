// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using DesktopApplication.Contracts.Views;
using DesktopApplication.Helpers;
using DesktopApplication.ViewModels.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DesktopApplication.Views.Forms
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransferFundsForm : Page, IDialogForm
    {
        public TransferFundsFormViewModel ViewModel { get; }

        private bool isValidTranferFromAccountSelection;
        private bool isValidTranferToAccountSelection;
        private bool isValidTransferAmountValue;
        private bool isSufficientTransferAmount;

        public TransferFundsForm()
        {
            ViewModel = App.GetService<TransferFundsFormViewModel>();
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadAsync();
        }

        public bool IsValidForm()
            => isValidTranferFromAccountSelection && isValidTranferToAccountSelection
            && isValidTransferAmountValue && isSufficientTransferAmount;

        public void SetModel(object model)
        {
            throw new NotImplementedException();
        }

        public void ValidateForm()
        {
            ValidateTransferFromAccountSelection();
            ValidateTransferToAccountSelection();
            ValidateTransferAmountValue();
            
            if (isValidTranferFromAccountSelection && isValidTransferAmountValue)
                ValidateTransferAmountSufficient();
        }

        private void ValidateTransferFromAccountSelection()
        {
            isValidTranferFromAccountSelection = FormValidator.ValidateSelection(ComboBoxTransferFrom.SelectedIndex);

            if (isValidTranferFromAccountSelection)
                HideInvalidTransferFromSelectionError();
            else
                ShowInvalidTranferFromSelectionError();
        }

        private void ValidateTransferToAccountSelection()
        {
            isValidTranferToAccountSelection = FormValidator.ValidateSelection(ComboBoxTransferTo.SelectedIndex);

            if (isValidTranferToAccountSelection)
                HideInvalidTransferToSelectionError();
            else
                ShowInvalidTransferToSelectionError();
        }

        private void ValidateTransferAmountValue()
        {
            isValidTransferAmountValue = FormValidator.ValidateDecimal(TextBoxTransferAmount.Text);

            if (isValidTransferAmountValue)
                HideInvalidTransferAmountValueError();
            else
                ShowInvalidTransferAmountValueError();
        }

        private void ValidateTransferAmountSufficient()
        {
            isSufficientTransferAmount = ViewModel.IsSufficientTransferAmount();

            if (isSufficientTransferAmount)
                HideInsufficientFundsError();
            else
                ShowInsufficientFundsError();
        }

        private void HideInvalidTransferFromSelectionError()
            => TBInvalidTranferFromSelectionError.Visibility = Visibility.Collapsed;

        private void HideInvalidTransferToSelectionError()
            => TBInvalidTranferToSelectionError.Visibility = Visibility.Collapsed;

        private void HideInvalidTransferAmountValueError()
            => TBInvalidTranferAmountError.Visibility = Visibility.Collapsed;

        private void HideInsufficientFundsError()
            => TBInsufficientFundsError.Visibility = Visibility.Collapsed;

        private void ShowInvalidTranferFromSelectionError()
            => TBInvalidTranferFromSelectionError.Visibility = Visibility.Visible;

        private void ShowInvalidTransferToSelectionError()
            => TBInvalidTranferToSelectionError.Visibility = Visibility.Visible;

        private void ShowInvalidTransferAmountValueError()
            => TBInvalidTranferAmountError.Visibility = Visibility.Visible;

        private void ShowInsufficientFundsError()
            => TBInsufficientFundsError.Visibility = Visibility.Visible;
    }
}
