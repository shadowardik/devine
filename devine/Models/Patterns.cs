using devine.Models;

public static class Patterns
{
    public static readonly string[] YaraRules =
    {
        "troxill.yar",
        "nixploit.yar",
        "m1rch_hb.yar",
        "suspicious.yar"
    };

    public static Dictionary<string, DetectionInfo> GetStringPatterns()
    {
        return new Dictionary<string, DetectionInfo>
        {
            // Generative
            {"areyoufuckingdump", new DetectionInfo("Generative [Obfuscation]")},
            {"T;T;T", new DetectionInfo("Generative [LVL 2]")},
            {"+#/#", new DetectionInfo("Generative [LVL 1]")},
            {"CRUNTIME144", new DetectionInfo("Generative [LVL 1]")},
            {"ALLATORI", new DetectionInfo("Generative [Obfuscation]")},
            {"zc.classm", new DetectionInfo("Generative [LVL 3]")},
            {"xP[\\", new DetectionInfo("Generative [LVL 1]")},
            {"Az85'", new DetectionInfo("Generative [LVL 1]")},
            {"d`elele\\epe", new DetectionInfo("Generative [LVL 2]")},
            {"HRCRHIIIq", new DetectionInfo("Generative [LVL 2]")},
            {"RHOOOCCC5.e", new DetectionInfo("Generative [LVL 2]")},
            {"3val$filter", new DetectionInfo("Generative [LVL 2]")},
            {"7zSIZE_LIMIT", new DetectionInfo("Generative [LVL 2]")},
            {"P__throw_concurrence_unlock_error", new DetectionInfo("Generative [LVL 3]")},
            {"|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||.class", new DetectionInfo("Generative [Obfuscation]")},
            {"!!5!27654'!5!27654'!5!", new DetectionInfo("Generative [LVL 3]")},

            // Cortex Client
            {"cortexuser1337", new DetectionInfo("Cortex client in instance")},
            {"cortexcreator", new DetectionInfo("Cortex client in instance")},
            {"cortexcreator1337", new DetectionInfo("Cortex client in instance")},
            {"cortexproduct0", new DetectionInfo("Cortex client in instance")},
            {"_^]++++", new DetectionInfo("Cortex client in instance")},
            // TODO: add new strings

            // Troxill Client
            {"trahil", new DetectionInfo("Troxill client in instance")},
            {"Troxill", new DetectionInfo("Troxill client in instance")},
            {"zdcoder", new DetectionInfo("Troxill client in instance")},
            {"IIv.class", new DetectionInfo("Troxill client in instance")},
            {"ADd.class", new DetectionInfo("Troxill client in instance")},
            {"PAZZAZZZY^Z^Y[_", new DetectionInfo("Troxill client in instance")},
            {"6L~gKlyLK(O*", new DetectionInfo("Troxill client in instance")},
            {"84AHc2&Mi%Z", new DetectionInfo("Troxill client in instance")},
            {"ZgYm[CUQRN", new DetectionInfo("Troxill client in instance")},
            {"VaJeoZvd@", new DetectionInfo("Troxill client in instance")},
            {"radioegor146", new DetectionInfo("Troxill client in instance")},

            // Doomsday
            {"\\+\\#/\\#", new DetectionInfo("Doomsday client in instance")},
            {"lVdoG]qYwDNd^StWom", new DetectionInfo("Doomsday client in instance")},
            {"aHWoMiUeB_ypx\\dqBvf", new DetectionInfo("Doomsday client in instance")},
            {"-1=-", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-2=-", new DetectionInfo("Doomsday client in instance")},
            {"-3=-", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-4=-", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-0=-", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-3-=", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-4-=", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-0-=", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-5-=", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"-9-=", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]5]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]4]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]3]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]2]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]1]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"]]9]", new DetectionInfo("Generative [LVL 3] [doomsday]")},
            {"\\*Nft", new DetectionInfo("Doomsday client in instance")},
            {"\\)JW8", new DetectionInfo("Doomsday client in instance")},
            {"SWAGOR_", new DetectionInfo("Doomsday client in instance")},
            {"1=Z>", new DetectionInfo("Doomsday client in instance")},
            {"4g31", new DetectionInfo("Doomsday client in instance")},
            {"*Wdk5lAQ4c*><slom9P", new DetectionInfo("Doomsday client in instance")},
            {"632A,><o%KsMB2", new DetectionInfo("Doomsday client in instance")},
            {"S>CtDG_yN1,><", new DetectionInfo("Doomsday client in instance")},
            {"T9t\\?1,", new DetectionInfo("Doomsday client in instance")},
            {".9Q6*WKZC#jwUl0", new DetectionInfo("Doomsday client in instance")},
            {"jtQRD^w_dSG^PZ", new DetectionInfo("Doomsday client in instance")},
            {"dddd::::", new DetectionInfo("Doomsday client in instance")},
            {"w)<@tvw", new DetectionInfo("Doomsday client in instance")},
            {"mmmmJJJJ", new DetectionInfo("Doomsday client in instance")},
            {"WOMD", new DetectionInfo("Doomsday client in instance")},

            // Other
            {"hitbsmodule", new DetectionInfo("Hitboxes in instance")},
            {"Noise Client", new DetectionInfo("Hitboxes in instance")},
            {"HASH_TICK", new DetectionInfo("Hitboxes in instance")},
            {"girlrent", new DetectionInfo("Dreampool in instance")},
            {"magicinject", new DetectionInfo("Dreampool in instance")},
            {"jarkasteal", new DetectionInfo("Jarka stealer in instance xD")},
            {"cmdcoders/cheats/", new DetectionInfo("Hitboxes in instance")},
            {"examplemod/hb", new DetectionInfo("Hitboxes in instance")},
            {"bus1root", new DetectionInfo("Hitboxes in instance")},
            {"me/bushroot/hb/gui", new DetectionInfo("Hitboxes in instance")},
            {"neathitbs", new DetectionInfo("Hitboxes in instance")},
            {"glowesp", new DetectionInfo("Hitboxes in instance")},
            {"dsf.webp", new DetectionInfo("Hitboxes in instance")},
            {"hitbs.setSize", new DetectionInfo("Hitboxes in instance")},
            {"chs/Profiller", new DetectionInfo("Hitboxes in instance")},
            {"waohitbs", new DetectionInfo("Hitboxes in instance")},
            {"5DIID_XMLDOMDocumentEvents", new DetectionInfo("Hitboxes in instance")},
            {"Derik1337", new DetectionInfo("Hitboxes in instance")},
            {"Exitension", new DetectionInfo("Hitboxes in instance")},
            {"listSpritesure", new DetectionInfo("Hitboxes in instance")},
            {"baobab", new DetectionInfo("Hitboxes in instance")},
            {"Zero/Time", new DetectionInfo("Hitboxes in instance")},
            {"BreakHitsOn", new DetectionInfo("Hitboxes in instance")},
            {"tickupdate", new DetectionInfo("Hitboxes in instance")},
            {"onupdate", new DetectionInfo("Hitboxes in instance")},
            {"ch0ffa_box", new DetectionInfo("Hitboxes in instance")},
            {"h1tb0x", new DetectionInfo("Hitboxes in instance")},
            {"oeshb", new DetectionInfo("Hitboxes in instance")},
            {"WHYMAKUD", new DetectionInfo("Hitboxes in instance")},
            {"rolleronbox", new DetectionInfo("Hitboxes in instance")},
            {"CREATED_BY_WHYMADUD", new DetectionInfo("Hitboxes in instance")},
            {"magictheinjecting", new DetectionInfo("Dreampool in instance")},
            {"*mEhk", new DetectionInfo("Hitboxes in instance")},
            {"VNejaWA.ModuleCh", new DetectionInfo("Rasty in instance")},
            {"bushrut", new DetectionInfo("Hitboxes in instance")},
            {"MagicInject", new DetectionInfo("Dreampool in instance")},
            {"clowdy", new DetectionInfo("Clowdy in instance")},
            {"lmao/drip/customhitbs", new DetectionInfo("Hitboxes in instance")},
            {"combat/hitbox", new DetectionInfo("Hitboxes in instance")},
            {"me/tsglu/ke/hitboxcommands", new DetectionInfo("Hitboxes in instance")},
            {"stubborn.website", new DetectionInfo("Hitboxes in instance")},
            {"ReachMe", new DetectionInfo("Hitboxes in instance")},
            {"SQUAD", new DetectionInfo("Hitboxes in instance")},
            {"net.minecraftforge.ASMEventHandler.31.wait(long, int)", new DetectionInfo("Hitboxes in instance")}
        };
    }

    public static Dictionary<string, DetectionInfo> GetRegexPatterns()
    {
        return new Dictionary<string, DetectionInfo>
    {
        // Doomsday
        {"^(?=.{10,15}$)(?!(.{5}=-)).*[^=-]=-[^=]{4}$", new DetectionInfo("Doomsday client in instance")},
        {"(?<=.{7})<>q", new DetectionInfo("Doomsday client in instance")},
        {"<>q(?=.{7})", new DetectionInfo("Doomsday client in instance")},
        {"7m=(?=.{4})", new DetectionInfo("Doomsday client in instance")},
        {"^(%|A|-%|W).*A%-", new DetectionInfo("Doomsday client in instance")},
        {"^.{6,40}A%-", new DetectionInfo("Doomsday client in instance")},
        // {"^.{6,}%Ou", new DetectionInfo("Doomsday client in instance")}, TODO: rework this regex
        {"V=-.*\\)L\\)5", new DetectionInfo("Doomsday client in instance")},
        {"\\[Ik\\$#w!aT@K\\]V-=J", new DetectionInfo("Doomsday client in instance")},
        {";34\\$2lMvw", new DetectionInfo("Doomsday client in instance")},
        {"kPi7,A\\$><NXJm", new DetectionInfo("Doomsday client in instance")},
        {"<>G!jO@%><", new DetectionInfo("Doomsday client in instance")},
        {"XV><\\^/_b`#bw4", new DetectionInfo("Doomsday client in instance")},
        {"O%-pU%Ks\\*,i1d@0", new DetectionInfo("Doomsday client in instance")},
        {"6d=Z:!LX0%-C", new DetectionInfo("Doomsday client in instance")},
        {"YVX&0%-Hi`>f!\"B", new DetectionInfo("Doomsday client in instance")},
        {"\\*WD(?=.{10,20})", new DetectionInfo("Doomsday client in instance")},
        {"(?<=.{10,20})\\*WD", new DetectionInfo("Doomsday client in instance")}
    };
    }
}