using System.Diagnostics.CodeAnalysis;

namespace BSE.Identity.Blazor.Client.ViewModels
{
    public class RoleViewModel// : IEqualityComparer<RoleViewModel>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected {  get; set; }

        //public override bool Equals(object? obj)
        //{
        //    if (obj is not RoleViewModel roleModel)
        //    {
        //        return false;
        //    }

        //    if (!Id.Equals(roleModel.Id))
        //    {
        //        return false;
        //    }
            
        //    if (!Name.Equals(roleModel.Name))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public bool Equals(RoleViewModel? x, RoleViewModel? y)
        //{
        //    throw new NotImplementedException();
        //}

        //public override int GetHashCode()
        //{
        //     return Id.GetHashCode();
        //}

        //public int GetHashCode([DisallowNull] RoleViewModel obj)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
