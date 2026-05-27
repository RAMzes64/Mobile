using CommunityToolkit.Mvvm.Input;
using mauiHello.Models;

namespace mauiHello.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}