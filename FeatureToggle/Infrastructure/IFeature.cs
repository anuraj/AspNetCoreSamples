namespace FeatureToggle.Infrastructure
{
    public interface IFeature
    {
        bool IsFeatureEnabled(string feature);
    }
}