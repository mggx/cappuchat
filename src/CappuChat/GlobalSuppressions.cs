[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1062:Validate arguments of public methods",
    Justification = @"Many APIs in here are public for runtime contract purposes (XAML etc.).
                    These are not however public APIs consumed by anyone other than the client they reside in."
)]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "We don't have any AppDomain shenanigans going on.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "This one is pretty stupid... Framework Design Guidelines needs to check their wording on this.")]

