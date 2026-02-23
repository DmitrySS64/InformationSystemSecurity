using InformationSystemSecurity.Tools;

const int collectN = 10000;
const string collectSeed = "САЛАМКА_БАЛАЛАМКА_БОРИСКА_СОСИСКА";
const char collectTarget = 'С';
const int collectPos = 7;

string[] cmdArgs = Environment.GetCommandLineArgs();

if (Array.Exists(cmdArgs, a => a == "--collect-hash-stats"))
{
    SpongeHashStatsCollector.CollectAndPrintStats(collectSeed, collectN, collectTarget, collectPos);
}
else
{
    Console.WriteLine("Hello, World!");
}
