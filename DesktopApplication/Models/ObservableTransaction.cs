using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ModelsLibrary;

namespace DesktopApplication.Models;
public class ObservableTransaction : ObservableObject
{
    private readonly Transaction _transaction;

    public ObservableTransaction(Transaction transaction)
    {
        _transaction = transaction;
    }
}
