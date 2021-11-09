namespace project_backend.Models.Utils
{
    public enum ClaimCtxTypes
    {
        Id
    }

    public static class ClaimCtxTypesExtension
    {
        public static string ToString(this ClaimCtxTypes claim)
        {
            return claim switch
            {
                ClaimCtxTypes.Id => "Id",
                _ => ""
            };
        }

    }
}
