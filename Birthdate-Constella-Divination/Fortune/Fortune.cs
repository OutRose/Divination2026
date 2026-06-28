namespace BirthdateConstellaDivination.Fortune
{
    public sealed record Fortune(int Life, int Gold, int Study, int Love, int Work, int Pattern)
    {
        public int MaxScoreCount =>
            (Life == FortuneConstants.MaxScore ? 1 : 0)
          + (Gold == FortuneConstants.MaxScore ? 1 : 0)
          + (Study == FortuneConstants.MaxScore ? 1 : 0)
          + (Love == FortuneConstants.MaxScore ? 1 : 0)
          + (Work == FortuneConstants.MaxScore ? 1 : 0)
          + (Pattern == FortuneConstants.MaxScore ? 1 : 0);

        public bool IsSuperLucky => MaxScoreCount >= FortuneConstants.SuperLuckyThreshold;
    }
}
