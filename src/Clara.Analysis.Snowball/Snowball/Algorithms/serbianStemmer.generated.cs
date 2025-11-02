// Generated from serbian.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from serbian.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class SerbianStemmer : Stemmer
    {
        private int I_p1;
        private bool B_no_diacritics;

        private const string g_v = "aeiou";
        private const string g_sa = "ćčđšž";
        private const string g_ca = "bcdfghjklmnprstvzćčđšž";
        private const string g_rg = "r";

        private static readonly Among[] a_0 = new[]
        {
            new Among("а", -1, 1, 0),
            new Among("б", -1, 2, 0),
            new Among("в", -1, 3, 0),
            new Among("г", -1, 4, 0),
            new Among("д", -1, 5, 0),
            new Among("е", -1, 7, 0),
            new Among("ж", -1, 8, 0),
            new Among("з", -1, 9, 0),
            new Among("и", -1, 10, 0),
            new Among("к", -1, 12, 0),
            new Among("л", -1, 13, 0),
            new Among("м", -1, 15, 0),
            new Among("н", -1, 16, 0),
            new Among("о", -1, 18, 0),
            new Among("п", -1, 19, 0),
            new Among("р", -1, 20, 0),
            new Among("с", -1, 21, 0),
            new Among("т", -1, 22, 0),
            new Among("у", -1, 24, 0),
            new Among("ф", -1, 25, 0),
            new Among("х", -1, 26, 0),
            new Among("ц", -1, 27, 0),
            new Among("ч", -1, 28, 0),
            new Among("ш", -1, 30, 0),
            new Among("ђ", -1, 6, 0),
            new Among("ј", -1, 11, 0),
            new Among("љ", -1, 14, 0),
            new Among("њ", -1, 17, 0),
            new Among("ћ", -1, 23, 0),
            new Among("џ", -1, 29, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("daba", -1, 73, 0),
            new Among("ajaca", -1, 12, 0),
            new Among("ejaca", -1, 14, 0),
            new Among("ljaca", -1, 13, 0),
            new Among("njaca", -1, 85, 0),
            new Among("ojaca", -1, 15, 0),
            new Among("alaca", -1, 82, 0),
            new Among("elaca", -1, 83, 0),
            new Among("olaca", -1, 84, 0),
            new Among("maca", -1, 75, 0),
            new Among("naca", -1, 76, 0),
            new Among("raca", -1, 81, 0),
            new Among("saca", -1, 80, 0),
            new Among("vaca", -1, 79, 0),
            new Among("šaca", -1, 18, 0),
            new Among("aoca", -1, 82, 0),
            new Among("acaka", -1, 55, 0),
            new Among("ajaka", -1, 16, 0),
            new Among("ojaka", -1, 17, 0),
            new Among("anaka", -1, 78, 0),
            new Among("ataka", -1, 58, 0),
            new Among("etaka", -1, 59, 0),
            new Among("itaka", -1, 60, 0),
            new Among("otaka", -1, 61, 0),
            new Among("utaka", -1, 62, 0),
            new Among("ačaka", -1, 54, 0),
            new Among("esama", -1, 67, 0),
            new Among("izama", -1, 87, 0),
            new Among("jacima", -1, 5, 0),
            new Among("nicima", -1, 23, 0),
            new Among("ticima", -1, 24, 0),
            new Among("teticima", 30, 21, 0),
            new Among("zicima", -1, 25, 0),
            new Among("atcima", -1, 58, 0),
            new Among("utcima", -1, 62, 0),
            new Among("čcima", -1, 74, 0),
            new Among("pesima", -1, 2, 0),
            new Among("inzima", -1, 19, 0),
            new Among("lozima", -1, 1, 0),
            new Among("metara", -1, 68, 0),
            new Among("centara", -1, 69, 0),
            new Among("istara", -1, 70, 0),
            new Among("ekata", -1, 86, 0),
            new Among("anata", -1, 53, 0),
            new Among("nstava", -1, 22, 0),
            new Among("kustava", -1, 29, 0),
            new Among("ajac", -1, 12, 0),
            new Among("ejac", -1, 14, 0),
            new Among("ljac", -1, 13, 0),
            new Among("njac", -1, 85, 0),
            new Among("anjac", 49, 11, 0),
            new Among("ojac", -1, 15, 0),
            new Among("alac", -1, 82, 0),
            new Among("elac", -1, 83, 0),
            new Among("olac", -1, 84, 0),
            new Among("mac", -1, 75, 0),
            new Among("nac", -1, 76, 0),
            new Among("rac", -1, 81, 0),
            new Among("sac", -1, 80, 0),
            new Among("vac", -1, 79, 0),
            new Among("šac", -1, 18, 0),
            new Among("jebe", -1, 88, 0),
            new Among("olce", -1, 84, 0),
            new Among("kuse", -1, 27, 0),
            new Among("rave", -1, 42, 0),
            new Among("save", -1, 52, 0),
            new Among("šave", -1, 51, 0),
            new Among("baci", -1, 89, 0),
            new Among("jaci", -1, 5, 0),
            new Among("tvenici", -1, 20, 0),
            new Among("snici", -1, 26, 0),
            new Among("tetici", -1, 21, 0),
            new Among("bojci", -1, 4, 0),
            new Among("vojci", -1, 3, 0),
            new Among("ojsci", -1, 66, 0),
            new Among("atci", -1, 58, 0),
            new Among("itci", -1, 60, 0),
            new Among("utci", -1, 62, 0),
            new Among("čci", -1, 74, 0),
            new Among("pesi", -1, 2, 0),
            new Among("inzi", -1, 19, 0),
            new Among("lozi", -1, 1, 0),
            new Among("acak", -1, 55, 0),
            new Among("usak", -1, 57, 0),
            new Among("atak", -1, 58, 0),
            new Among("etak", -1, 59, 0),
            new Among("itak", -1, 60, 0),
            new Among("otak", -1, 61, 0),
            new Among("utak", -1, 62, 0),
            new Among("ačak", -1, 54, 0),
            new Among("ušak", -1, 56, 0),
            new Among("izam", -1, 87, 0),
            new Among("tican", -1, 65, 0),
            new Among("cajan", -1, 7, 0),
            new Among("čajan", -1, 6, 0),
            new Among("voljan", -1, 77, 0),
            new Among("eskan", -1, 63, 0),
            new Among("alan", -1, 40, 0),
            new Among("bilan", -1, 33, 0),
            new Among("gilan", -1, 37, 0),
            new Among("nilan", -1, 39, 0),
            new Among("rilan", -1, 38, 0),
            new Among("silan", -1, 36, 0),
            new Among("tilan", -1, 34, 0),
            new Among("avilan", -1, 35, 0),
            new Among("laran", -1, 9, 0),
            new Among("eran", -1, 8, 0),
            new Among("asan", -1, 91, 0),
            new Among("esan", -1, 10, 0),
            new Among("dusan", -1, 31, 0),
            new Among("kusan", -1, 28, 0),
            new Among("atan", -1, 47, 0),
            new Among("pletan", -1, 50, 0),
            new Among("tetan", -1, 49, 0),
            new Among("antan", -1, 32, 0),
            new Among("pravan", -1, 44, 0),
            new Among("stavan", -1, 43, 0),
            new Among("sivan", -1, 46, 0),
            new Among("tivan", -1, 45, 0),
            new Among("ozan", -1, 41, 0),
            new Among("tičan", -1, 64, 0),
            new Among("ašan", -1, 90, 0),
            new Among("dušan", -1, 30, 0),
            new Among("metar", -1, 68, 0),
            new Among("centar", -1, 69, 0),
            new Among("istar", -1, 70, 0),
            new Among("ekat", -1, 86, 0),
            new Among("enat", -1, 48, 0),
            new Among("oscu", -1, 72, 0),
            new Among("ošću", -1, 71, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("aca", -1, 124, 0),
            new Among("eca", -1, 125, 0),
            new Among("uca", -1, 126, 0),
            new Among("ga", -1, 20, 0),
            new Among("acega", 3, 124, 0),
            new Among("ecega", 3, 125, 0),
            new Among("ucega", 3, 126, 0),
            new Among("anjijega", 3, 84, 0),
            new Among("enjijega", 3, 85, 0),
            new Among("snjijega", 3, 122, 0),
            new Among("šnjijega", 3, 86, 0),
            new Among("kijega", 3, 95, 0),
            new Among("skijega", 11, 1, 0),
            new Among("škijega", 11, 2, 0),
            new Among("elijega", 3, 83, 0),
            new Among("nijega", 3, 13, 0),
            new Among("osijega", 3, 123, 0),
            new Among("atijega", 3, 120, 0),
            new Among("evitijega", 3, 92, 0),
            new Among("ovitijega", 3, 93, 0),
            new Among("astijega", 3, 94, 0),
            new Among("avijega", 3, 77, 0),
            new Among("evijega", 3, 78, 0),
            new Among("ivijega", 3, 79, 0),
            new Among("ovijega", 3, 80, 0),
            new Among("ošijega", 3, 91, 0),
            new Among("anjega", 3, 84, 0),
            new Among("enjega", 3, 85, 0),
            new Among("snjega", 3, 122, 0),
            new Among("šnjega", 3, 86, 0),
            new Among("kega", 3, 95, 0),
            new Among("skega", 30, 1, 0),
            new Among("škega", 30, 2, 0),
            new Among("elega", 3, 83, 0),
            new Among("nega", 3, 13, 0),
            new Among("anega", 34, 10, 0),
            new Among("enega", 34, 87, 0),
            new Among("snega", 34, 159, 0),
            new Among("šnega", 34, 88, 0),
            new Among("osega", 3, 123, 0),
            new Among("atega", 3, 120, 0),
            new Among("evitega", 3, 92, 0),
            new Among("ovitega", 3, 93, 0),
            new Among("astega", 3, 94, 0),
            new Among("avega", 3, 77, 0),
            new Among("evega", 3, 78, 0),
            new Among("ivega", 3, 79, 0),
            new Among("ovega", 3, 80, 0),
            new Among("aćega", 3, 14, 0),
            new Among("ećega", 3, 15, 0),
            new Among("ućega", 3, 16, 0),
            new Among("ošega", 3, 91, 0),
            new Among("acoga", 3, 124, 0),
            new Among("ecoga", 3, 125, 0),
            new Among("ucoga", 3, 126, 0),
            new Among("anjoga", 3, 84, 0),
            new Among("enjoga", 3, 85, 0),
            new Among("snjoga", 3, 122, 0),
            new Among("šnjoga", 3, 86, 0),
            new Among("koga", 3, 95, 0),
            new Among("skoga", 59, 1, 0),
            new Among("škoga", 59, 2, 0),
            new Among("loga", 3, 19, 0),
            new Among("eloga", 62, 83, 0),
            new Among("noga", 3, 13, 0),
            new Among("cinoga", 64, 137, 0),
            new Among("činoga", 64, 89, 0),
            new Among("osoga", 3, 123, 0),
            new Among("atoga", 3, 120, 0),
            new Among("evitoga", 3, 92, 0),
            new Among("ovitoga", 3, 93, 0),
            new Among("astoga", 3, 94, 0),
            new Among("avoga", 3, 77, 0),
            new Among("evoga", 3, 78, 0),
            new Among("ivoga", 3, 79, 0),
            new Among("ovoga", 3, 80, 0),
            new Among("aćoga", 3, 14, 0),
            new Among("ećoga", 3, 15, 0),
            new Among("ućoga", 3, 16, 0),
            new Among("ošoga", 3, 91, 0),
            new Among("uga", 3, 18, 0),
            new Among("aja", -1, 109, 0),
            new Among("caja", 81, 26, 0),
            new Among("laja", 81, 30, 0),
            new Among("raja", 81, 31, 0),
            new Among("ćaja", 81, 28, 0),
            new Among("čaja", 81, 27, 0),
            new Among("đaja", 81, 29, 0),
            new Among("bija", -1, 32, 0),
            new Among("cija", -1, 33, 0),
            new Among("dija", -1, 34, 0),
            new Among("fija", -1, 40, 0),
            new Among("gija", -1, 39, 0),
            new Among("anjija", -1, 84, 0),
            new Among("enjija", -1, 85, 0),
            new Among("snjija", -1, 122, 0),
            new Among("šnjija", -1, 86, 0),
            new Among("kija", -1, 95, 0),
            new Among("skija", 97, 1, 0),
            new Among("škija", 97, 2, 0),
            new Among("lija", -1, 24, 0),
            new Among("elija", 100, 83, 0),
            new Among("mija", -1, 37, 0),
            new Among("nija", -1, 13, 0),
            new Among("ganija", 103, 9, 0),
            new Among("manija", 103, 6, 0),
            new Among("panija", 103, 7, 0),
            new Among("ranija", 103, 8, 0),
            new Among("tanija", 103, 5, 0),
            new Among("pija", -1, 41, 0),
            new Among("rija", -1, 42, 0),
            new Among("rarija", 110, 21, 0),
            new Among("sija", -1, 23, 0),
            new Among("osija", 112, 123, 0),
            new Among("tija", -1, 44, 0),
            new Among("atija", 114, 120, 0),
            new Among("evitija", 114, 92, 0),
            new Among("ovitija", 114, 93, 0),
            new Among("otija", 114, 22, 0),
            new Among("astija", 114, 94, 0),
            new Among("avija", -1, 77, 0),
            new Among("evija", -1, 78, 0),
            new Among("ivija", -1, 79, 0),
            new Among("ovija", -1, 80, 0),
            new Among("zija", -1, 45, 0),
            new Among("ošija", -1, 91, 0),
            new Among("žija", -1, 38, 0),
            new Among("anja", -1, 84, 0),
            new Among("enja", -1, 85, 0),
            new Among("snja", -1, 122, 0),
            new Among("šnja", -1, 86, 0),
            new Among("ka", -1, 95, 0),
            new Among("ska", 131, 1, 0),
            new Among("ška", 131, 2, 0),
            new Among("ala", -1, 104, 0),
            new Among("acala", 134, 128, 0),
            new Among("astajala", 134, 106, 0),
            new Among("istajala", 134, 107, 0),
            new Among("ostajala", 134, 108, 0),
            new Among("ijala", 134, 47, 0),
            new Among("injala", 134, 114, 0),
            new Among("nala", 134, 46, 0),
            new Among("irala", 134, 100, 0),
            new Among("urala", 134, 105, 0),
            new Among("tala", 134, 113, 0),
            new Among("astala", 144, 110, 0),
            new Among("istala", 144, 111, 0),
            new Among("ostala", 144, 112, 0),
            new Among("avala", 134, 97, 0),
            new Among("evala", 134, 96, 0),
            new Among("ivala", 134, 98, 0),
            new Among("ovala", 134, 76, 0),
            new Among("uvala", 134, 99, 0),
            new Among("ačala", 134, 102, 0),
            new Among("ela", -1, 83, 0),
            new Among("ila", -1, 116, 0),
            new Among("acila", 155, 124, 0),
            new Among("lucila", 155, 121, 0),
            new Among("nila", 155, 103, 0),
            new Among("astanila", 158, 110, 0),
            new Among("istanila", 158, 111, 0),
            new Among("ostanila", 158, 112, 0),
            new Among("rosila", 155, 127, 0),
            new Among("jetila", 155, 118, 0),
            new Among("ozila", 155, 48, 0),
            new Among("ačila", 155, 101, 0),
            new Among("lučila", 155, 117, 0),
            new Among("rošila", 155, 90, 0),
            new Among("ola", -1, 50, 0),
            new Among("asla", -1, 115, 0),
            new Among("nula", -1, 13, 0),
            new Among("gama", -1, 20, 0),
            new Among("logama", 171, 19, 0),
            new Among("ugama", 171, 18, 0),
            new Among("ajama", -1, 109, 0),
            new Among("cajama", 174, 26, 0),
            new Among("lajama", 174, 30, 0),
            new Among("rajama", 174, 31, 0),
            new Among("ćajama", 174, 28, 0),
            new Among("čajama", 174, 27, 0),
            new Among("đajama", 174, 29, 0),
            new Among("bijama", -1, 32, 0),
            new Among("cijama", -1, 33, 0),
            new Among("dijama", -1, 34, 0),
            new Among("fijama", -1, 40, 0),
            new Among("gijama", -1, 39, 0),
            new Among("lijama", -1, 35, 0),
            new Among("mijama", -1, 37, 0),
            new Among("nijama", -1, 36, 0),
            new Among("ganijama", 188, 9, 0),
            new Among("manijama", 188, 6, 0),
            new Among("panijama", 188, 7, 0),
            new Among("ranijama", 188, 8, 0),
            new Among("tanijama", 188, 5, 0),
            new Among("pijama", -1, 41, 0),
            new Among("rijama", -1, 42, 0),
            new Among("sijama", -1, 43, 0),
            new Among("tijama", -1, 44, 0),
            new Among("zijama", -1, 45, 0),
            new Among("žijama", -1, 38, 0),
            new Among("alama", -1, 104, 0),
            new Among("ijalama", 200, 47, 0),
            new Among("nalama", 200, 46, 0),
            new Among("elama", -1, 119, 0),
            new Among("ilama", -1, 116, 0),
            new Among("ramama", -1, 52, 0),
            new Among("lemama", -1, 51, 0),
            new Among("inama", -1, 11, 0),
            new Among("cinama", 207, 137, 0),
            new Among("činama", 207, 89, 0),
            new Among("rama", -1, 52, 0),
            new Among("arama", 210, 53, 0),
            new Among("drama", 210, 54, 0),
            new Among("erama", 210, 55, 0),
            new Among("orama", 210, 56, 0),
            new Among("basama", -1, 135, 0),
            new Among("gasama", -1, 131, 0),
            new Among("jasama", -1, 129, 0),
            new Among("kasama", -1, 133, 0),
            new Among("nasama", -1, 132, 0),
            new Among("tasama", -1, 130, 0),
            new Among("vasama", -1, 134, 0),
            new Among("esama", -1, 152, 0),
            new Among("isama", -1, 154, 0),
            new Among("etama", -1, 70, 0),
            new Among("estama", -1, 71, 0),
            new Among("istama", -1, 72, 0),
            new Among("kstama", -1, 73, 0),
            new Among("ostama", -1, 74, 0),
            new Among("avama", -1, 77, 0),
            new Among("evama", -1, 78, 0),
            new Among("ivama", -1, 79, 0),
            new Among("bašama", -1, 63, 0),
            new Among("gašama", -1, 64, 0),
            new Among("jašama", -1, 61, 0),
            new Among("kašama", -1, 62, 0),
            new Among("našama", -1, 60, 0),
            new Among("tašama", -1, 59, 0),
            new Among("vašama", -1, 65, 0),
            new Among("ešama", -1, 66, 0),
            new Among("išama", -1, 67, 0),
            new Among("lema", -1, 51, 0),
            new Among("acima", -1, 124, 0),
            new Among("ecima", -1, 125, 0),
            new Among("ucima", -1, 126, 0),
            new Among("ajima", -1, 109, 0),
            new Among("cajima", 245, 26, 0),
            new Among("lajima", 245, 30, 0),
            new Among("rajima", 245, 31, 0),
            new Among("ćajima", 245, 28, 0),
            new Among("čajima", 245, 27, 0),
            new Among("đajima", 245, 29, 0),
            new Among("bijima", -1, 32, 0),
            new Among("cijima", -1, 33, 0),
            new Among("dijima", -1, 34, 0),
            new Among("fijima", -1, 40, 0),
            new Among("gijima", -1, 39, 0),
            new Among("anjijima", -1, 84, 0),
            new Among("enjijima", -1, 85, 0),
            new Among("snjijima", -1, 122, 0),
            new Among("šnjijima", -1, 86, 0),
            new Among("kijima", -1, 95, 0),
            new Among("skijima", 261, 1, 0),
            new Among("škijima", 261, 2, 0),
            new Among("lijima", -1, 35, 0),
            new Among("elijima", 264, 83, 0),
            new Among("mijima", -1, 37, 0),
            new Among("nijima", -1, 13, 0),
            new Among("ganijima", 267, 9, 0),
            new Among("manijima", 267, 6, 0),
            new Among("panijima", 267, 7, 0),
            new Among("ranijima", 267, 8, 0),
            new Among("tanijima", 267, 5, 0),
            new Among("pijima", -1, 41, 0),
            new Among("rijima", -1, 42, 0),
            new Among("sijima", -1, 43, 0),
            new Among("osijima", 275, 123, 0),
            new Among("tijima", -1, 44, 0),
            new Among("atijima", 277, 120, 0),
            new Among("evitijima", 277, 92, 0),
            new Among("ovitijima", 277, 93, 0),
            new Among("astijima", 277, 94, 0),
            new Among("avijima", -1, 77, 0),
            new Among("evijima", -1, 78, 0),
            new Among("ivijima", -1, 79, 0),
            new Among("ovijima", -1, 80, 0),
            new Among("zijima", -1, 45, 0),
            new Among("ošijima", -1, 91, 0),
            new Among("žijima", -1, 38, 0),
            new Among("anjima", -1, 84, 0),
            new Among("enjima", -1, 85, 0),
            new Among("snjima", -1, 122, 0),
            new Among("šnjima", -1, 86, 0),
            new Among("kima", -1, 95, 0),
            new Among("skima", 293, 1, 0),
            new Among("škima", 293, 2, 0),
            new Among("alima", -1, 104, 0),
            new Among("ijalima", 296, 47, 0),
            new Among("nalima", 296, 46, 0),
            new Among("elima", -1, 83, 0),
            new Among("ilima", -1, 116, 0),
            new Among("ozilima", 300, 48, 0),
            new Among("olima", -1, 50, 0),
            new Among("lemima", -1, 51, 0),
            new Among("nima", -1, 13, 0),
            new Among("anima", 304, 10, 0),
            new Among("inima", 304, 11, 0),
            new Among("cinima", 306, 137, 0),
            new Among("činima", 306, 89, 0),
            new Among("onima", 304, 12, 0),
            new Among("arima", -1, 53, 0),
            new Among("drima", -1, 54, 0),
            new Among("erima", -1, 55, 0),
            new Among("orima", -1, 56, 0),
            new Among("basima", -1, 135, 0),
            new Among("gasima", -1, 131, 0),
            new Among("jasima", -1, 129, 0),
            new Among("kasima", -1, 133, 0),
            new Among("nasima", -1, 132, 0),
            new Among("tasima", -1, 130, 0),
            new Among("vasima", -1, 134, 0),
            new Among("esima", -1, 57, 0),
            new Among("isima", -1, 58, 0),
            new Among("osima", -1, 123, 0),
            new Among("atima", -1, 120, 0),
            new Among("ikatima", 324, 68, 0),
            new Among("latima", 324, 69, 0),
            new Among("etima", -1, 70, 0),
            new Among("evitima", -1, 92, 0),
            new Among("ovitima", -1, 93, 0),
            new Among("astima", -1, 94, 0),
            new Among("estima", -1, 71, 0),
            new Among("istima", -1, 72, 0),
            new Among("kstima", -1, 73, 0),
            new Among("ostima", -1, 74, 0),
            new Among("ištima", -1, 75, 0),
            new Among("avima", -1, 77, 0),
            new Among("evima", -1, 78, 0),
            new Among("ajevima", 337, 109, 0),
            new Among("cajevima", 338, 26, 0),
            new Among("lajevima", 338, 30, 0),
            new Among("rajevima", 338, 31, 0),
            new Among("ćajevima", 338, 28, 0),
            new Among("čajevima", 338, 27, 0),
            new Among("đajevima", 338, 29, 0),
            new Among("ivima", -1, 79, 0),
            new Among("ovima", -1, 80, 0),
            new Among("govima", 346, 20, 0),
            new Among("ugovima", 347, 17, 0),
            new Among("lovima", 346, 82, 0),
            new Among("olovima", 349, 49, 0),
            new Among("movima", 346, 81, 0),
            new Among("onovima", 346, 12, 0),
            new Among("stvima", -1, 3, 0),
            new Among("štvima", -1, 4, 0),
            new Among("aćima", -1, 14, 0),
            new Among("ećima", -1, 15, 0),
            new Among("ućima", -1, 16, 0),
            new Among("bašima", -1, 63, 0),
            new Among("gašima", -1, 64, 0),
            new Among("jašima", -1, 61, 0),
            new Among("kašima", -1, 62, 0),
            new Among("našima", -1, 60, 0),
            new Among("tašima", -1, 59, 0),
            new Among("vašima", -1, 65, 0),
            new Among("ešima", -1, 66, 0),
            new Among("išima", -1, 67, 0),
            new Among("ošima", -1, 91, 0),
            new Among("na", -1, 13, 0),
            new Among("ana", 368, 10, 0),
            new Among("acana", 369, 128, 0),
            new Among("urana", 369, 105, 0),
            new Among("tana", 369, 113, 0),
            new Among("avana", 369, 97, 0),
            new Among("evana", 369, 96, 0),
            new Among("ivana", 369, 98, 0),
            new Among("uvana", 369, 99, 0),
            new Among("ačana", 369, 102, 0),
            new Among("acena", 368, 124, 0),
            new Among("lucena", 368, 121, 0),
            new Among("ačena", 368, 101, 0),
            new Among("lučena", 368, 117, 0),
            new Among("ina", 368, 11, 0),
            new Among("cina", 382, 137, 0),
            new Among("anina", 382, 10, 0),
            new Among("čina", 382, 89, 0),
            new Among("ona", 368, 12, 0),
            new Among("ara", -1, 53, 0),
            new Among("dra", -1, 54, 0),
            new Among("era", -1, 55, 0),
            new Among("ora", -1, 56, 0),
            new Among("basa", -1, 135, 0),
            new Among("gasa", -1, 131, 0),
            new Among("jasa", -1, 129, 0),
            new Among("kasa", -1, 133, 0),
            new Among("nasa", -1, 132, 0),
            new Among("tasa", -1, 130, 0),
            new Among("vasa", -1, 134, 0),
            new Among("esa", -1, 57, 0),
            new Among("isa", -1, 58, 0),
            new Among("osa", -1, 123, 0),
            new Among("ata", -1, 120, 0),
            new Among("ikata", 401, 68, 0),
            new Among("lata", 401, 69, 0),
            new Among("eta", -1, 70, 0),
            new Among("evita", -1, 92, 0),
            new Among("ovita", -1, 93, 0),
            new Among("asta", -1, 94, 0),
            new Among("esta", -1, 71, 0),
            new Among("ista", -1, 72, 0),
            new Among("ksta", -1, 73, 0),
            new Among("osta", -1, 74, 0),
            new Among("nuta", -1, 13, 0),
            new Among("išta", -1, 75, 0),
            new Among("ava", -1, 77, 0),
            new Among("eva", -1, 78, 0),
            new Among("ajeva", 415, 109, 0),
            new Among("cajeva", 416, 26, 0),
            new Among("lajeva", 416, 30, 0),
            new Among("rajeva", 416, 31, 0),
            new Among("ćajeva", 416, 28, 0),
            new Among("čajeva", 416, 27, 0),
            new Among("đajeva", 416, 29, 0),
            new Among("iva", -1, 79, 0),
            new Among("ova", -1, 80, 0),
            new Among("gova", 424, 20, 0),
            new Among("ugova", 425, 17, 0),
            new Among("lova", 424, 82, 0),
            new Among("olova", 427, 49, 0),
            new Among("mova", 424, 81, 0),
            new Among("onova", 424, 12, 0),
            new Among("stva", -1, 3, 0),
            new Among("štva", -1, 4, 0),
            new Among("aća", -1, 14, 0),
            new Among("eća", -1, 15, 0),
            new Among("uća", -1, 16, 0),
            new Among("baša", -1, 63, 0),
            new Among("gaša", -1, 64, 0),
            new Among("jaša", -1, 61, 0),
            new Among("kaša", -1, 62, 0),
            new Among("naša", -1, 60, 0),
            new Among("taša", -1, 59, 0),
            new Among("vaša", -1, 65, 0),
            new Among("eša", -1, 66, 0),
            new Among("iša", -1, 67, 0),
            new Among("oša", -1, 91, 0),
            new Among("ace", -1, 124, 0),
            new Among("ece", -1, 125, 0),
            new Among("uce", -1, 126, 0),
            new Among("luce", 448, 121, 0),
            new Among("astade", -1, 110, 0),
            new Among("istade", -1, 111, 0),
            new Among("ostade", -1, 112, 0),
            new Among("ge", -1, 20, 0),
            new Among("loge", 453, 19, 0),
            new Among("uge", 453, 18, 0),
            new Among("aje", -1, 104, 0),
            new Among("caje", 456, 26, 0),
            new Among("laje", 456, 30, 0),
            new Among("raje", 456, 31, 0),
            new Among("astaje", 456, 106, 0),
            new Among("istaje", 456, 107, 0),
            new Among("ostaje", 456, 108, 0),
            new Among("ćaje", 456, 28, 0),
            new Among("čaje", 456, 27, 0),
            new Among("đaje", 456, 29, 0),
            new Among("ije", -1, 116, 0),
            new Among("bije", 466, 32, 0),
            new Among("cije", 466, 33, 0),
            new Among("dije", 466, 34, 0),
            new Among("fije", 466, 40, 0),
            new Among("gije", 466, 39, 0),
            new Among("anjije", 466, 84, 0),
            new Among("enjije", 466, 85, 0),
            new Among("snjije", 466, 122, 0),
            new Among("šnjije", 466, 86, 0),
            new Among("kije", 466, 95, 0),
            new Among("skije", 476, 1, 0),
            new Among("škije", 476, 2, 0),
            new Among("lije", 466, 35, 0),
            new Among("elije", 479, 83, 0),
            new Among("mije", 466, 37, 0),
            new Among("nije", 466, 13, 0),
            new Among("ganije", 482, 9, 0),
            new Among("manije", 482, 6, 0),
            new Among("panije", 482, 7, 0),
            new Among("ranije", 482, 8, 0),
            new Among("tanije", 482, 5, 0),
            new Among("pije", 466, 41, 0),
            new Among("rije", 466, 42, 0),
            new Among("sije", 466, 43, 0),
            new Among("osije", 490, 123, 0),
            new Among("tije", 466, 44, 0),
            new Among("atije", 492, 120, 0),
            new Among("evitije", 492, 92, 0),
            new Among("ovitije", 492, 93, 0),
            new Among("astije", 492, 94, 0),
            new Among("avije", 466, 77, 0),
            new Among("evije", 466, 78, 0),
            new Among("ivije", 466, 79, 0),
            new Among("ovije", 466, 80, 0),
            new Among("zije", 466, 45, 0),
            new Among("ošije", 466, 91, 0),
            new Among("žije", 466, 38, 0),
            new Among("anje", -1, 84, 0),
            new Among("enje", -1, 85, 0),
            new Among("snje", -1, 122, 0),
            new Among("šnje", -1, 86, 0),
            new Among("uje", -1, 25, 0),
            new Among("lucuje", 508, 121, 0),
            new Among("iruje", 508, 100, 0),
            new Among("lučuje", 508, 117, 0),
            new Among("ke", -1, 95, 0),
            new Among("ske", 512, 1, 0),
            new Among("ške", 512, 2, 0),
            new Among("ale", -1, 104, 0),
            new Among("acale", 515, 128, 0),
            new Among("astajale", 515, 106, 0),
            new Among("istajale", 515, 107, 0),
            new Among("ostajale", 515, 108, 0),
            new Among("ijale", 515, 47, 0),
            new Among("injale", 515, 114, 0),
            new Among("nale", 515, 46, 0),
            new Among("irale", 515, 100, 0),
            new Among("urale", 515, 105, 0),
            new Among("tale", 515, 113, 0),
            new Among("astale", 525, 110, 0),
            new Among("istale", 525, 111, 0),
            new Among("ostale", 525, 112, 0),
            new Among("avale", 515, 97, 0),
            new Among("evale", 515, 96, 0),
            new Among("ivale", 515, 98, 0),
            new Among("ovale", 515, 76, 0),
            new Among("uvale", 515, 99, 0),
            new Among("ačale", 515, 102, 0),
            new Among("ele", -1, 83, 0),
            new Among("ile", -1, 116, 0),
            new Among("acile", 536, 124, 0),
            new Among("lucile", 536, 121, 0),
            new Among("nile", 536, 103, 0),
            new Among("rosile", 536, 127, 0),
            new Among("jetile", 536, 118, 0),
            new Among("ozile", 536, 48, 0),
            new Among("ačile", 536, 101, 0),
            new Among("lučile", 536, 117, 0),
            new Among("rošile", 536, 90, 0),
            new Among("ole", -1, 50, 0),
            new Among("asle", -1, 115, 0),
            new Among("nule", -1, 13, 0),
            new Among("rame", -1, 52, 0),
            new Among("leme", -1, 51, 0),
            new Among("acome", -1, 124, 0),
            new Among("ecome", -1, 125, 0),
            new Among("ucome", -1, 126, 0),
            new Among("anjome", -1, 84, 0),
            new Among("enjome", -1, 85, 0),
            new Among("snjome", -1, 122, 0),
            new Among("šnjome", -1, 86, 0),
            new Among("kome", -1, 95, 0),
            new Among("skome", 558, 1, 0),
            new Among("škome", 558, 2, 0),
            new Among("elome", -1, 83, 0),
            new Among("nome", -1, 13, 0),
            new Among("cinome", 562, 137, 0),
            new Among("činome", 562, 89, 0),
            new Among("osome", -1, 123, 0),
            new Among("atome", -1, 120, 0),
            new Among("evitome", -1, 92, 0),
            new Among("ovitome", -1, 93, 0),
            new Among("astome", -1, 94, 0),
            new Among("avome", -1, 77, 0),
            new Among("evome", -1, 78, 0),
            new Among("ivome", -1, 79, 0),
            new Among("ovome", -1, 80, 0),
            new Among("aćome", -1, 14, 0),
            new Among("ećome", -1, 15, 0),
            new Among("ućome", -1, 16, 0),
            new Among("ošome", -1, 91, 0),
            new Among("ne", -1, 13, 0),
            new Among("ane", 578, 10, 0),
            new Among("acane", 579, 128, 0),
            new Among("urane", 579, 105, 0),
            new Among("tane", 579, 113, 0),
            new Among("astane", 582, 110, 0),
            new Among("istane", 582, 111, 0),
            new Among("ostane", 582, 112, 0),
            new Among("avane", 579, 97, 0),
            new Among("evane", 579, 96, 0),
            new Among("ivane", 579, 98, 0),
            new Among("uvane", 579, 99, 0),
            new Among("ačane", 579, 102, 0),
            new Among("acene", 578, 124, 0),
            new Among("lucene", 578, 121, 0),
            new Among("ačene", 578, 101, 0),
            new Among("lučene", 578, 117, 0),
            new Among("ine", 578, 11, 0),
            new Among("cine", 595, 137, 0),
            new Among("anine", 595, 10, 0),
            new Among("čine", 595, 89, 0),
            new Among("one", 578, 12, 0),
            new Among("are", -1, 53, 0),
            new Among("dre", -1, 54, 0),
            new Among("ere", -1, 55, 0),
            new Among("ore", -1, 56, 0),
            new Among("ase", -1, 161, 0),
            new Among("base", 604, 135, 0),
            new Among("acase", 604, 128, 0),
            new Among("gase", 604, 131, 0),
            new Among("jase", 604, 129, 0),
            new Among("astajase", 608, 138, 0),
            new Among("istajase", 608, 139, 0),
            new Among("ostajase", 608, 140, 0),
            new Among("injase", 608, 150, 0),
            new Among("kase", 604, 133, 0),
            new Among("nase", 604, 132, 0),
            new Among("irase", 604, 155, 0),
            new Among("urase", 604, 156, 0),
            new Among("tase", 604, 130, 0),
            new Among("vase", 604, 134, 0),
            new Among("avase", 618, 144, 0),
            new Among("evase", 618, 145, 0),
            new Among("ivase", 618, 146, 0),
            new Among("ovase", 618, 148, 0),
            new Among("uvase", 618, 147, 0),
            new Among("ese", -1, 57, 0),
            new Among("ise", -1, 58, 0),
            new Among("acise", 625, 124, 0),
            new Among("lucise", 625, 121, 0),
            new Among("rosise", 625, 127, 0),
            new Among("jetise", 625, 149, 0),
            new Among("ose", -1, 123, 0),
            new Among("astadose", 630, 141, 0),
            new Among("istadose", 630, 142, 0),
            new Among("ostadose", 630, 143, 0),
            new Among("ate", -1, 104, 0),
            new Among("acate", 634, 128, 0),
            new Among("ikate", 634, 68, 0),
            new Among("late", 634, 69, 0),
            new Among("irate", 634, 100, 0),
            new Among("urate", 634, 105, 0),
            new Among("tate", 634, 113, 0),
            new Among("avate", 634, 97, 0),
            new Among("evate", 634, 96, 0),
            new Among("ivate", 634, 98, 0),
            new Among("uvate", 634, 99, 0),
            new Among("ačate", 634, 102, 0),
            new Among("ete", -1, 70, 0),
            new Among("astadete", 646, 110, 0),
            new Among("istadete", 646, 111, 0),
            new Among("ostadete", 646, 112, 0),
            new Among("astajete", 646, 106, 0),
            new Among("istajete", 646, 107, 0),
            new Among("ostajete", 646, 108, 0),
            new Among("ijete", 646, 116, 0),
            new Among("injete", 646, 114, 0),
            new Among("ujete", 646, 25, 0),
            new Among("lucujete", 655, 121, 0),
            new Among("irujete", 655, 100, 0),
            new Among("lučujete", 655, 117, 0),
            new Among("nete", 646, 13, 0),
            new Among("astanete", 659, 110, 0),
            new Among("istanete", 659, 111, 0),
            new Among("ostanete", 659, 112, 0),
            new Among("astete", 646, 115, 0),
            new Among("ite", -1, 116, 0),
            new Among("acite", 664, 124, 0),
            new Among("lucite", 664, 121, 0),
            new Among("nite", 664, 13, 0),
            new Among("astanite", 667, 110, 0),
            new Among("istanite", 667, 111, 0),
            new Among("ostanite", 667, 112, 0),
            new Among("rosite", 664, 127, 0),
            new Among("jetite", 664, 118, 0),
            new Among("astite", 664, 115, 0),
            new Among("evite", 664, 92, 0),
            new Among("ovite", 664, 93, 0),
            new Among("ačite", 664, 101, 0),
            new Among("lučite", 664, 117, 0),
            new Among("rošite", 664, 90, 0),
            new Among("ajte", -1, 104, 0),
            new Among("urajte", 679, 105, 0),
            new Among("tajte", 679, 113, 0),
            new Among("astajte", 681, 106, 0),
            new Among("istajte", 681, 107, 0),
            new Among("ostajte", 681, 108, 0),
            new Among("avajte", 679, 97, 0),
            new Among("evajte", 679, 96, 0),
            new Among("ivajte", 679, 98, 0),
            new Among("uvajte", 679, 99, 0),
            new Among("ijte", -1, 116, 0),
            new Among("lucujte", -1, 121, 0),
            new Among("irujte", -1, 100, 0),
            new Among("lučujte", -1, 117, 0),
            new Among("aste", -1, 94, 0),
            new Among("acaste", 693, 128, 0),
            new Among("astajaste", 693, 106, 0),
            new Among("istajaste", 693, 107, 0),
            new Among("ostajaste", 693, 108, 0),
            new Among("injaste", 693, 114, 0),
            new Among("iraste", 693, 100, 0),
            new Among("uraste", 693, 105, 0),
            new Among("taste", 693, 113, 0),
            new Among("avaste", 693, 97, 0),
            new Among("evaste", 693, 96, 0),
            new Among("ivaste", 693, 98, 0),
            new Among("ovaste", 693, 76, 0),
            new Among("uvaste", 693, 99, 0),
            new Among("ačaste", 693, 102, 0),
            new Among("este", -1, 71, 0),
            new Among("iste", -1, 72, 0),
            new Among("aciste", 709, 124, 0),
            new Among("luciste", 709, 121, 0),
            new Among("niste", 709, 103, 0),
            new Among("rosiste", 709, 127, 0),
            new Among("jetiste", 709, 118, 0),
            new Among("ačiste", 709, 101, 0),
            new Among("lučiste", 709, 117, 0),
            new Among("rošiste", 709, 90, 0),
            new Among("kste", -1, 73, 0),
            new Among("oste", -1, 74, 0),
            new Among("astadoste", 719, 110, 0),
            new Among("istadoste", 719, 111, 0),
            new Among("ostadoste", 719, 112, 0),
            new Among("nuste", -1, 13, 0),
            new Among("ište", -1, 75, 0),
            new Among("ave", -1, 77, 0),
            new Among("eve", -1, 78, 0),
            new Among("ajeve", 726, 109, 0),
            new Among("cajeve", 727, 26, 0),
            new Among("lajeve", 727, 30, 0),
            new Among("rajeve", 727, 31, 0),
            new Among("ćajeve", 727, 28, 0),
            new Among("čajeve", 727, 27, 0),
            new Among("đajeve", 727, 29, 0),
            new Among("ive", -1, 79, 0),
            new Among("ove", -1, 80, 0),
            new Among("gove", 735, 20, 0),
            new Among("ugove", 736, 17, 0),
            new Among("love", 735, 82, 0),
            new Among("olove", 738, 49, 0),
            new Among("move", 735, 81, 0),
            new Among("onove", 735, 12, 0),
            new Among("aće", -1, 14, 0),
            new Among("eće", -1, 15, 0),
            new Among("uće", -1, 16, 0),
            new Among("ače", -1, 101, 0),
            new Among("luče", -1, 117, 0),
            new Among("aše", -1, 104, 0),
            new Among("baše", 747, 63, 0),
            new Among("gaše", 747, 64, 0),
            new Among("jaše", 747, 61, 0),
            new Among("astajaše", 750, 106, 0),
            new Among("istajaše", 750, 107, 0),
            new Among("ostajaše", 750, 108, 0),
            new Among("injaše", 750, 114, 0),
            new Among("kaše", 747, 62, 0),
            new Among("naše", 747, 60, 0),
            new Among("iraše", 747, 100, 0),
            new Among("uraše", 747, 105, 0),
            new Among("taše", 747, 59, 0),
            new Among("vaše", 747, 65, 0),
            new Among("avaše", 760, 97, 0),
            new Among("evaše", 760, 96, 0),
            new Among("ivaše", 760, 98, 0),
            new Among("ovaše", 760, 76, 0),
            new Among("uvaše", 760, 99, 0),
            new Among("ačaše", 747, 102, 0),
            new Among("eše", -1, 66, 0),
            new Among("iše", -1, 67, 0),
            new Among("jetiše", 768, 118, 0),
            new Among("ačiše", 768, 101, 0),
            new Among("lučiše", 768, 117, 0),
            new Among("rošiše", 768, 90, 0),
            new Among("oše", -1, 91, 0),
            new Among("astadoše", 773, 110, 0),
            new Among("istadoše", 773, 111, 0),
            new Among("ostadoše", 773, 112, 0),
            new Among("aceg", -1, 124, 0),
            new Among("eceg", -1, 125, 0),
            new Among("uceg", -1, 126, 0),
            new Among("anjijeg", -1, 84, 0),
            new Among("enjijeg", -1, 85, 0),
            new Among("snjijeg", -1, 122, 0),
            new Among("šnjijeg", -1, 86, 0),
            new Among("kijeg", -1, 95, 0),
            new Among("skijeg", 784, 1, 0),
            new Among("škijeg", 784, 2, 0),
            new Among("elijeg", -1, 83, 0),
            new Among("nijeg", -1, 13, 0),
            new Among("osijeg", -1, 123, 0),
            new Among("atijeg", -1, 120, 0),
            new Among("evitijeg", -1, 92, 0),
            new Among("ovitijeg", -1, 93, 0),
            new Among("astijeg", -1, 94, 0),
            new Among("avijeg", -1, 77, 0),
            new Among("evijeg", -1, 78, 0),
            new Among("ivijeg", -1, 79, 0),
            new Among("ovijeg", -1, 80, 0),
            new Among("ošijeg", -1, 91, 0),
            new Among("anjeg", -1, 84, 0),
            new Among("enjeg", -1, 85, 0),
            new Among("snjeg", -1, 122, 0),
            new Among("šnjeg", -1, 86, 0),
            new Among("keg", -1, 95, 0),
            new Among("eleg", -1, 83, 0),
            new Among("neg", -1, 13, 0),
            new Among("aneg", 805, 10, 0),
            new Among("eneg", 805, 87, 0),
            new Among("sneg", 805, 159, 0),
            new Among("šneg", 805, 88, 0),
            new Among("oseg", -1, 123, 0),
            new Among("ateg", -1, 120, 0),
            new Among("aveg", -1, 77, 0),
            new Among("eveg", -1, 78, 0),
            new Among("iveg", -1, 79, 0),
            new Among("oveg", -1, 80, 0),
            new Among("aćeg", -1, 14, 0),
            new Among("ećeg", -1, 15, 0),
            new Among("ućeg", -1, 16, 0),
            new Among("ošeg", -1, 91, 0),
            new Among("acog", -1, 124, 0),
            new Among("ecog", -1, 125, 0),
            new Among("ucog", -1, 126, 0),
            new Among("anjog", -1, 84, 0),
            new Among("enjog", -1, 85, 0),
            new Among("snjog", -1, 122, 0),
            new Among("šnjog", -1, 86, 0),
            new Among("kog", -1, 95, 0),
            new Among("skog", 827, 1, 0),
            new Among("škog", 827, 2, 0),
            new Among("elog", -1, 83, 0),
            new Among("nog", -1, 13, 0),
            new Among("cinog", 831, 137, 0),
            new Among("činog", 831, 89, 0),
            new Among("osog", -1, 123, 0),
            new Among("atog", -1, 120, 0),
            new Among("evitog", -1, 92, 0),
            new Among("ovitog", -1, 93, 0),
            new Among("astog", -1, 94, 0),
            new Among("avog", -1, 77, 0),
            new Among("evog", -1, 78, 0),
            new Among("ivog", -1, 79, 0),
            new Among("ovog", -1, 80, 0),
            new Among("aćog", -1, 14, 0),
            new Among("ećog", -1, 15, 0),
            new Among("ućog", -1, 16, 0),
            new Among("ošog", -1, 91, 0),
            new Among("ah", -1, 104, 0),
            new Among("acah", 847, 128, 0),
            new Among("astajah", 847, 106, 0),
            new Among("istajah", 847, 107, 0),
            new Among("ostajah", 847, 108, 0),
            new Among("injah", 847, 114, 0),
            new Among("irah", 847, 100, 0),
            new Among("urah", 847, 105, 0),
            new Among("tah", 847, 113, 0),
            new Among("avah", 847, 97, 0),
            new Among("evah", 847, 96, 0),
            new Among("ivah", 847, 98, 0),
            new Among("ovah", 847, 76, 0),
            new Among("uvah", 847, 99, 0),
            new Among("ačah", 847, 102, 0),
            new Among("ih", -1, 116, 0),
            new Among("acih", 862, 124, 0),
            new Among("ecih", 862, 125, 0),
            new Among("ucih", 862, 126, 0),
            new Among("lucih", 865, 121, 0),
            new Among("anjijih", 862, 84, 0),
            new Among("enjijih", 862, 85, 0),
            new Among("snjijih", 862, 122, 0),
            new Among("šnjijih", 862, 86, 0),
            new Among("kijih", 862, 95, 0),
            new Among("skijih", 871, 1, 0),
            new Among("škijih", 871, 2, 0),
            new Among("elijih", 862, 83, 0),
            new Among("nijih", 862, 13, 0),
            new Among("osijih", 862, 123, 0),
            new Among("atijih", 862, 120, 0),
            new Among("evitijih", 862, 92, 0),
            new Among("ovitijih", 862, 93, 0),
            new Among("astijih", 862, 94, 0),
            new Among("avijih", 862, 77, 0),
            new Among("evijih", 862, 78, 0),
            new Among("ivijih", 862, 79, 0),
            new Among("ovijih", 862, 80, 0),
            new Among("ošijih", 862, 91, 0),
            new Among("anjih", 862, 84, 0),
            new Among("enjih", 862, 85, 0),
            new Among("snjih", 862, 122, 0),
            new Among("šnjih", 862, 86, 0),
            new Among("kih", 862, 95, 0),
            new Among("skih", 890, 1, 0),
            new Among("ških", 890, 2, 0),
            new Among("elih", 862, 83, 0),
            new Among("nih", 862, 13, 0),
            new Among("cinih", 894, 137, 0),
            new Among("činih", 894, 89, 0),
            new Among("osih", 862, 123, 0),
            new Among("rosih", 897, 127, 0),
            new Among("atih", 862, 120, 0),
            new Among("jetih", 862, 118, 0),
            new Among("evitih", 862, 92, 0),
            new Among("ovitih", 862, 93, 0),
            new Among("astih", 862, 94, 0),
            new Among("avih", 862, 77, 0),
            new Among("evih", 862, 78, 0),
            new Among("ivih", 862, 79, 0),
            new Among("ovih", 862, 80, 0),
            new Among("aćih", 862, 14, 0),
            new Among("ećih", 862, 15, 0),
            new Among("ućih", 862, 16, 0),
            new Among("ačih", 862, 101, 0),
            new Among("lučih", 862, 117, 0),
            new Among("oših", 862, 91, 0),
            new Among("roših", 913, 90, 0),
            new Among("astadoh", -1, 110, 0),
            new Among("istadoh", -1, 111, 0),
            new Among("ostadoh", -1, 112, 0),
            new Among("acuh", -1, 124, 0),
            new Among("ecuh", -1, 125, 0),
            new Among("ucuh", -1, 126, 0),
            new Among("aćuh", -1, 14, 0),
            new Among("ećuh", -1, 15, 0),
            new Among("ućuh", -1, 16, 0),
            new Among("aci", -1, 124, 0),
            new Among("aceci", -1, 124, 0),
            new Among("ieci", -1, 162, 0),
            new Among("ajuci", -1, 161, 0),
            new Among("irajuci", 927, 155, 0),
            new Among("urajuci", 927, 156, 0),
            new Among("astajuci", 927, 138, 0),
            new Among("istajuci", 927, 139, 0),
            new Among("ostajuci", 927, 140, 0),
            new Among("avajuci", 927, 144, 0),
            new Among("evajuci", 927, 145, 0),
            new Among("ivajuci", 927, 146, 0),
            new Among("uvajuci", 927, 147, 0),
            new Among("ujuci", -1, 157, 0),
            new Among("lucujuci", 937, 121, 0),
            new Among("irujuci", 937, 155, 0),
            new Among("luci", -1, 121, 0),
            new Among("nuci", -1, 164, 0),
            new Among("etuci", -1, 153, 0),
            new Among("astuci", -1, 136, 0),
            new Among("gi", -1, 20, 0),
            new Among("ugi", 944, 18, 0),
            new Among("aji", -1, 109, 0),
            new Among("caji", 946, 26, 0),
            new Among("laji", 946, 30, 0),
            new Among("raji", 946, 31, 0),
            new Among("ćaji", 946, 28, 0),
            new Among("čaji", 946, 27, 0),
            new Among("đaji", 946, 29, 0),
            new Among("biji", -1, 32, 0),
            new Among("ciji", -1, 33, 0),
            new Among("diji", -1, 34, 0),
            new Among("fiji", -1, 40, 0),
            new Among("giji", -1, 39, 0),
            new Among("anjiji", -1, 84, 0),
            new Among("enjiji", -1, 85, 0),
            new Among("snjiji", -1, 122, 0),
            new Among("šnjiji", -1, 86, 0),
            new Among("kiji", -1, 95, 0),
            new Among("skiji", 962, 1, 0),
            new Among("škiji", 962, 2, 0),
            new Among("liji", -1, 35, 0),
            new Among("eliji", 965, 83, 0),
            new Among("miji", -1, 37, 0),
            new Among("niji", -1, 13, 0),
            new Among("ganiji", 968, 9, 0),
            new Among("maniji", 968, 6, 0),
            new Among("paniji", 968, 7, 0),
            new Among("raniji", 968, 8, 0),
            new Among("taniji", 968, 5, 0),
            new Among("piji", -1, 41, 0),
            new Among("riji", -1, 42, 0),
            new Among("siji", -1, 43, 0),
            new Among("osiji", 976, 123, 0),
            new Among("tiji", -1, 44, 0),
            new Among("atiji", 978, 120, 0),
            new Among("evitiji", 978, 92, 0),
            new Among("ovitiji", 978, 93, 0),
            new Among("astiji", 978, 94, 0),
            new Among("aviji", -1, 77, 0),
            new Among("eviji", -1, 78, 0),
            new Among("iviji", -1, 79, 0),
            new Among("oviji", -1, 80, 0),
            new Among("ziji", -1, 45, 0),
            new Among("ošiji", -1, 91, 0),
            new Among("žiji", -1, 38, 0),
            new Among("anji", -1, 84, 0),
            new Among("enji", -1, 85, 0),
            new Among("snji", -1, 122, 0),
            new Among("šnji", -1, 86, 0),
            new Among("ki", -1, 95, 0),
            new Among("ski", 994, 1, 0),
            new Among("ški", 994, 2, 0),
            new Among("ali", -1, 104, 0),
            new Among("acali", 997, 128, 0),
            new Among("astajali", 997, 106, 0),
            new Among("istajali", 997, 107, 0),
            new Among("ostajali", 997, 108, 0),
            new Among("ijali", 997, 47, 0),
            new Among("injali", 997, 114, 0),
            new Among("nali", 997, 46, 0),
            new Among("irali", 997, 100, 0),
            new Among("urali", 997, 105, 0),
            new Among("tali", 997, 113, 0),
            new Among("astali", 1007, 110, 0),
            new Among("istali", 1007, 111, 0),
            new Among("ostali", 1007, 112, 0),
            new Among("avali", 997, 97, 0),
            new Among("evali", 997, 96, 0),
            new Among("ivali", 997, 98, 0),
            new Among("ovali", 997, 76, 0),
            new Among("uvali", 997, 99, 0),
            new Among("ačali", 997, 102, 0),
            new Among("eli", -1, 83, 0),
            new Among("ili", -1, 116, 0),
            new Among("acili", 1018, 124, 0),
            new Among("lucili", 1018, 121, 0),
            new Among("nili", 1018, 103, 0),
            new Among("rosili", 1018, 127, 0),
            new Among("jetili", 1018, 118, 0),
            new Among("ozili", 1018, 48, 0),
            new Among("ačili", 1018, 101, 0),
            new Among("lučili", 1018, 117, 0),
            new Among("rošili", 1018, 90, 0),
            new Among("oli", -1, 50, 0),
            new Among("asli", -1, 115, 0),
            new Among("nuli", -1, 13, 0),
            new Among("rami", -1, 52, 0),
            new Among("lemi", -1, 51, 0),
            new Among("ni", -1, 13, 0),
            new Among("ani", 1033, 10, 0),
            new Among("acani", 1034, 128, 0),
            new Among("urani", 1034, 105, 0),
            new Among("tani", 1034, 113, 0),
            new Among("avani", 1034, 97, 0),
            new Among("evani", 1034, 96, 0),
            new Among("ivani", 1034, 98, 0),
            new Among("uvani", 1034, 99, 0),
            new Among("ačani", 1034, 102, 0),
            new Among("aceni", 1033, 124, 0),
            new Among("luceni", 1033, 121, 0),
            new Among("ačeni", 1033, 101, 0),
            new Among("lučeni", 1033, 117, 0),
            new Among("ini", 1033, 11, 0),
            new Among("cini", 1047, 137, 0),
            new Among("čini", 1047, 89, 0),
            new Among("oni", 1033, 12, 0),
            new Among("ari", -1, 53, 0),
            new Among("dri", -1, 54, 0),
            new Among("eri", -1, 55, 0),
            new Among("ori", -1, 56, 0),
            new Among("basi", -1, 135, 0),
            new Among("gasi", -1, 131, 0),
            new Among("jasi", -1, 129, 0),
            new Among("kasi", -1, 133, 0),
            new Among("nasi", -1, 132, 0),
            new Among("tasi", -1, 130, 0),
            new Among("vasi", -1, 134, 0),
            new Among("esi", -1, 152, 0),
            new Among("isi", -1, 154, 0),
            new Among("osi", -1, 123, 0),
            new Among("avsi", -1, 161, 0),
            new Among("acavsi", 1065, 128, 0),
            new Among("iravsi", 1065, 155, 0),
            new Among("tavsi", 1065, 160, 0),
            new Among("etavsi", 1068, 153, 0),
            new Among("astavsi", 1068, 141, 0),
            new Among("istavsi", 1068, 142, 0),
            new Among("ostavsi", 1068, 143, 0),
            new Among("ivsi", -1, 162, 0),
            new Among("nivsi", 1073, 158, 0),
            new Among("rosivsi", 1073, 127, 0),
            new Among("nuvsi", -1, 164, 0),
            new Among("ati", -1, 104, 0),
            new Among("acati", 1077, 128, 0),
            new Among("astajati", 1077, 106, 0),
            new Among("istajati", 1077, 107, 0),
            new Among("ostajati", 1077, 108, 0),
            new Among("injati", 1077, 114, 0),
            new Among("ikati", 1077, 68, 0),
            new Among("lati", 1077, 69, 0),
            new Among("irati", 1077, 100, 0),
            new Among("urati", 1077, 105, 0),
            new Among("tati", 1077, 113, 0),
            new Among("astati", 1087, 110, 0),
            new Among("istati", 1087, 111, 0),
            new Among("ostati", 1087, 112, 0),
            new Among("avati", 1077, 97, 0),
            new Among("evati", 1077, 96, 0),
            new Among("ivati", 1077, 98, 0),
            new Among("ovati", 1077, 76, 0),
            new Among("uvati", 1077, 99, 0),
            new Among("ačati", 1077, 102, 0),
            new Among("eti", -1, 70, 0),
            new Among("iti", -1, 116, 0),
            new Among("aciti", 1098, 124, 0),
            new Among("luciti", 1098, 121, 0),
            new Among("niti", 1098, 103, 0),
            new Among("rositi", 1098, 127, 0),
            new Among("jetiti", 1098, 118, 0),
            new Among("eviti", 1098, 92, 0),
            new Among("oviti", 1098, 93, 0),
            new Among("ačiti", 1098, 101, 0),
            new Among("lučiti", 1098, 117, 0),
            new Among("rošiti", 1098, 90, 0),
            new Among("asti", -1, 94, 0),
            new Among("esti", -1, 71, 0),
            new Among("isti", -1, 72, 0),
            new Among("ksti", -1, 73, 0),
            new Among("osti", -1, 74, 0),
            new Among("nuti", -1, 13, 0),
            new Among("avi", -1, 77, 0),
            new Among("evi", -1, 78, 0),
            new Among("ajevi", 1116, 109, 0),
            new Among("cajevi", 1117, 26, 0),
            new Among("lajevi", 1117, 30, 0),
            new Among("rajevi", 1117, 31, 0),
            new Among("ćajevi", 1117, 28, 0),
            new Among("čajevi", 1117, 27, 0),
            new Among("đajevi", 1117, 29, 0),
            new Among("ivi", -1, 79, 0),
            new Among("ovi", -1, 80, 0),
            new Among("govi", 1125, 20, 0),
            new Among("ugovi", 1126, 17, 0),
            new Among("lovi", 1125, 82, 0),
            new Among("olovi", 1128, 49, 0),
            new Among("movi", 1125, 81, 0),
            new Among("onovi", 1125, 12, 0),
            new Among("ieći", -1, 116, 0),
            new Among("ačeći", -1, 101, 0),
            new Among("ajući", -1, 104, 0),
            new Among("irajući", 1134, 100, 0),
            new Among("urajući", 1134, 105, 0),
            new Among("astajući", 1134, 106, 0),
            new Among("istajući", 1134, 107, 0),
            new Among("ostajući", 1134, 108, 0),
            new Among("avajući", 1134, 97, 0),
            new Among("evajući", 1134, 96, 0),
            new Among("ivajući", 1134, 98, 0),
            new Among("uvajući", 1134, 99, 0),
            new Among("ujući", -1, 25, 0),
            new Among("irujući", 1144, 100, 0),
            new Among("lučujući", 1144, 117, 0),
            new Among("nući", -1, 13, 0),
            new Among("etući", -1, 70, 0),
            new Among("astući", -1, 115, 0),
            new Among("ači", -1, 101, 0),
            new Among("luči", -1, 117, 0),
            new Among("baši", -1, 63, 0),
            new Among("gaši", -1, 64, 0),
            new Among("jaši", -1, 61, 0),
            new Among("kaši", -1, 62, 0),
            new Among("naši", -1, 60, 0),
            new Among("taši", -1, 59, 0),
            new Among("vaši", -1, 65, 0),
            new Among("eši", -1, 66, 0),
            new Among("iši", -1, 67, 0),
            new Among("oši", -1, 91, 0),
            new Among("avši", -1, 104, 0),
            new Among("iravši", 1162, 100, 0),
            new Among("tavši", 1162, 113, 0),
            new Among("etavši", 1164, 70, 0),
            new Among("astavši", 1164, 110, 0),
            new Among("istavši", 1164, 111, 0),
            new Among("ostavši", 1164, 112, 0),
            new Among("ačavši", 1162, 102, 0),
            new Among("ivši", -1, 116, 0),
            new Among("nivši", 1170, 103, 0),
            new Among("rošivši", 1170, 90, 0),
            new Among("nuvši", -1, 13, 0),
            new Among("aj", -1, 104, 0),
            new Among("uraj", 1174, 105, 0),
            new Among("taj", 1174, 113, 0),
            new Among("avaj", 1174, 97, 0),
            new Among("evaj", 1174, 96, 0),
            new Among("ivaj", 1174, 98, 0),
            new Among("uvaj", 1174, 99, 0),
            new Among("ij", -1, 116, 0),
            new Among("acoj", -1, 124, 0),
            new Among("ecoj", -1, 125, 0),
            new Among("ucoj", -1, 126, 0),
            new Among("anjijoj", -1, 84, 0),
            new Among("enjijoj", -1, 85, 0),
            new Among("snjijoj", -1, 122, 0),
            new Among("šnjijoj", -1, 86, 0),
            new Among("kijoj", -1, 95, 0),
            new Among("skijoj", 1189, 1, 0),
            new Among("škijoj", 1189, 2, 0),
            new Among("elijoj", -1, 83, 0),
            new Among("nijoj", -1, 13, 0),
            new Among("osijoj", -1, 123, 0),
            new Among("evitijoj", -1, 92, 0),
            new Among("ovitijoj", -1, 93, 0),
            new Among("astijoj", -1, 94, 0),
            new Among("avijoj", -1, 77, 0),
            new Among("evijoj", -1, 78, 0),
            new Among("ivijoj", -1, 79, 0),
            new Among("ovijoj", -1, 80, 0),
            new Among("ošijoj", -1, 91, 0),
            new Among("anjoj", -1, 84, 0),
            new Among("enjoj", -1, 85, 0),
            new Among("snjoj", -1, 122, 0),
            new Among("šnjoj", -1, 86, 0),
            new Among("koj", -1, 95, 0),
            new Among("skoj", 1207, 1, 0),
            new Among("škoj", 1207, 2, 0),
            new Among("aloj", -1, 104, 0),
            new Among("eloj", -1, 83, 0),
            new Among("noj", -1, 13, 0),
            new Among("cinoj", 1212, 137, 0),
            new Among("činoj", 1212, 89, 0),
            new Among("osoj", -1, 123, 0),
            new Among("atoj", -1, 120, 0),
            new Among("evitoj", -1, 92, 0),
            new Among("ovitoj", -1, 93, 0),
            new Among("astoj", -1, 94, 0),
            new Among("avoj", -1, 77, 0),
            new Among("evoj", -1, 78, 0),
            new Among("ivoj", -1, 79, 0),
            new Among("ovoj", -1, 80, 0),
            new Among("aćoj", -1, 14, 0),
            new Among("ećoj", -1, 15, 0),
            new Among("ućoj", -1, 16, 0),
            new Among("ošoj", -1, 91, 0),
            new Among("lucuj", -1, 121, 0),
            new Among("iruj", -1, 100, 0),
            new Among("lučuj", -1, 117, 0),
            new Among("al", -1, 104, 0),
            new Among("iral", 1231, 100, 0),
            new Among("ural", 1231, 105, 0),
            new Among("el", -1, 119, 0),
            new Among("il", -1, 116, 0),
            new Among("am", -1, 104, 0),
            new Among("acam", 1236, 128, 0),
            new Among("iram", 1236, 100, 0),
            new Among("uram", 1236, 105, 0),
            new Among("tam", 1236, 113, 0),
            new Among("avam", 1236, 97, 0),
            new Among("evam", 1236, 96, 0),
            new Among("ivam", 1236, 98, 0),
            new Among("uvam", 1236, 99, 0),
            new Among("ačam", 1236, 102, 0),
            new Among("em", -1, 119, 0),
            new Among("acem", 1246, 124, 0),
            new Among("ecem", 1246, 125, 0),
            new Among("ucem", 1246, 126, 0),
            new Among("astadem", 1246, 110, 0),
            new Among("istadem", 1246, 111, 0),
            new Among("ostadem", 1246, 112, 0),
            new Among("ajem", 1246, 104, 0),
            new Among("cajem", 1253, 26, 0),
            new Among("lajem", 1253, 30, 0),
            new Among("rajem", 1253, 31, 0),
            new Among("astajem", 1253, 106, 0),
            new Among("istajem", 1253, 107, 0),
            new Among("ostajem", 1253, 108, 0),
            new Among("ćajem", 1253, 28, 0),
            new Among("čajem", 1253, 27, 0),
            new Among("đajem", 1253, 29, 0),
            new Among("ijem", 1246, 116, 0),
            new Among("anjijem", 1263, 84, 0),
            new Among("enjijem", 1263, 85, 0),
            new Among("snjijem", 1263, 123, 0),
            new Among("šnjijem", 1263, 86, 0),
            new Among("kijem", 1263, 95, 0),
            new Among("skijem", 1268, 1, 0),
            new Among("škijem", 1268, 2, 0),
            new Among("lijem", 1263, 24, 0),
            new Among("elijem", 1271, 83, 0),
            new Among("nijem", 1263, 13, 0),
            new Among("rarijem", 1263, 21, 0),
            new Among("sijem", 1263, 23, 0),
            new Among("osijem", 1275, 123, 0),
            new Among("atijem", 1263, 120, 0),
            new Among("evitijem", 1263, 92, 0),
            new Among("ovitijem", 1263, 93, 0),
            new Among("otijem", 1263, 22, 0),
            new Among("astijem", 1263, 94, 0),
            new Among("avijem", 1263, 77, 0),
            new Among("evijem", 1263, 78, 0),
            new Among("ivijem", 1263, 79, 0),
            new Among("ovijem", 1263, 80, 0),
            new Among("ošijem", 1263, 91, 0),
            new Among("anjem", 1246, 84, 0),
            new Among("enjem", 1246, 85, 0),
            new Among("injem", 1246, 114, 0),
            new Among("snjem", 1246, 122, 0),
            new Among("šnjem", 1246, 86, 0),
            new Among("ujem", 1246, 25, 0),
            new Among("lucujem", 1292, 121, 0),
            new Among("irujem", 1292, 100, 0),
            new Among("lučujem", 1292, 117, 0),
            new Among("kem", 1246, 95, 0),
            new Among("skem", 1296, 1, 0),
            new Among("škem", 1296, 2, 0),
            new Among("elem", 1246, 83, 0),
            new Among("nem", 1246, 13, 0),
            new Among("anem", 1300, 10, 0),
            new Among("astanem", 1301, 110, 0),
            new Among("istanem", 1301, 111, 0),
            new Among("ostanem", 1301, 112, 0),
            new Among("enem", 1300, 87, 0),
            new Among("snem", 1300, 159, 0),
            new Among("šnem", 1300, 88, 0),
            new Among("basem", 1246, 135, 0),
            new Among("gasem", 1246, 131, 0),
            new Among("jasem", 1246, 129, 0),
            new Among("kasem", 1246, 133, 0),
            new Among("nasem", 1246, 132, 0),
            new Among("tasem", 1246, 130, 0),
            new Among("vasem", 1246, 134, 0),
            new Among("esem", 1246, 152, 0),
            new Among("isem", 1246, 154, 0),
            new Among("osem", 1246, 123, 0),
            new Among("atem", 1246, 120, 0),
            new Among("etem", 1246, 70, 0),
            new Among("evitem", 1246, 92, 0),
            new Among("ovitem", 1246, 93, 0),
            new Among("astem", 1246, 94, 0),
            new Among("istem", 1246, 151, 0),
            new Among("ištem", 1246, 75, 0),
            new Among("avem", 1246, 77, 0),
            new Among("evem", 1246, 78, 0),
            new Among("ivem", 1246, 79, 0),
            new Among("aćem", 1246, 14, 0),
            new Among("ećem", 1246, 15, 0),
            new Among("ućem", 1246, 16, 0),
            new Among("bašem", 1246, 63, 0),
            new Among("gašem", 1246, 64, 0),
            new Among("jašem", 1246, 61, 0),
            new Among("kašem", 1246, 62, 0),
            new Among("našem", 1246, 60, 0),
            new Among("tašem", 1246, 59, 0),
            new Among("vašem", 1246, 65, 0),
            new Among("ešem", 1246, 66, 0),
            new Among("išem", 1246, 67, 0),
            new Among("ošem", 1246, 91, 0),
            new Among("im", -1, 116, 0),
            new Among("acim", 1341, 124, 0),
            new Among("ecim", 1341, 125, 0),
            new Among("ucim", 1341, 126, 0),
            new Among("lucim", 1344, 121, 0),
            new Among("anjijim", 1341, 84, 0),
            new Among("enjijim", 1341, 85, 0),
            new Among("snjijim", 1341, 122, 0),
            new Among("šnjijim", 1341, 86, 0),
            new Among("kijim", 1341, 95, 0),
            new Among("skijim", 1350, 1, 0),
            new Among("škijim", 1350, 2, 0),
            new Among("elijim", 1341, 83, 0),
            new Among("nijim", 1341, 13, 0),
            new Among("osijim", 1341, 123, 0),
            new Among("atijim", 1341, 120, 0),
            new Among("evitijim", 1341, 92, 0),
            new Among("ovitijim", 1341, 93, 0),
            new Among("astijim", 1341, 94, 0),
            new Among("avijim", 1341, 77, 0),
            new Among("evijim", 1341, 78, 0),
            new Among("ivijim", 1341, 79, 0),
            new Among("ovijim", 1341, 80, 0),
            new Among("ošijim", 1341, 91, 0),
            new Among("anjim", 1341, 84, 0),
            new Among("enjim", 1341, 85, 0),
            new Among("snjim", 1341, 122, 0),
            new Among("šnjim", 1341, 86, 0),
            new Among("kim", 1341, 95, 0),
            new Among("skim", 1369, 1, 0),
            new Among("škim", 1369, 2, 0),
            new Among("elim", 1341, 83, 0),
            new Among("nim", 1341, 13, 0),
            new Among("cinim", 1373, 137, 0),
            new Among("činim", 1373, 89, 0),
            new Among("osim", 1341, 123, 0),
            new Among("rosim", 1376, 127, 0),
            new Among("atim", 1341, 120, 0),
            new Among("jetim", 1341, 118, 0),
            new Among("evitim", 1341, 92, 0),
            new Among("ovitim", 1341, 93, 0),
            new Among("astim", 1341, 94, 0),
            new Among("avim", 1341, 77, 0),
            new Among("evim", 1341, 78, 0),
            new Among("ivim", 1341, 79, 0),
            new Among("ovim", 1341, 80, 0),
            new Among("aćim", 1341, 14, 0),
            new Among("ećim", 1341, 15, 0),
            new Among("ućim", 1341, 16, 0),
            new Among("ačim", 1341, 101, 0),
            new Among("lučim", 1341, 117, 0),
            new Among("ošim", 1341, 91, 0),
            new Among("rošim", 1392, 90, 0),
            new Among("acom", -1, 124, 0),
            new Among("ecom", -1, 125, 0),
            new Among("ucom", -1, 126, 0),
            new Among("gom", -1, 20, 0),
            new Among("logom", 1397, 19, 0),
            new Among("ugom", 1397, 18, 0),
            new Among("bijom", -1, 32, 0),
            new Among("cijom", -1, 33, 0),
            new Among("dijom", -1, 34, 0),
            new Among("fijom", -1, 40, 0),
            new Among("gijom", -1, 39, 0),
            new Among("lijom", -1, 35, 0),
            new Among("mijom", -1, 37, 0),
            new Among("nijom", -1, 36, 0),
            new Among("ganijom", 1407, 9, 0),
            new Among("manijom", 1407, 6, 0),
            new Among("panijom", 1407, 7, 0),
            new Among("ranijom", 1407, 8, 0),
            new Among("tanijom", 1407, 5, 0),
            new Among("pijom", -1, 41, 0),
            new Among("rijom", -1, 42, 0),
            new Among("sijom", -1, 43, 0),
            new Among("tijom", -1, 44, 0),
            new Among("zijom", -1, 45, 0),
            new Among("žijom", -1, 38, 0),
            new Among("anjom", -1, 84, 0),
            new Among("enjom", -1, 85, 0),
            new Among("snjom", -1, 122, 0),
            new Among("šnjom", -1, 86, 0),
            new Among("kom", -1, 95, 0),
            new Among("skom", 1423, 1, 0),
            new Among("škom", 1423, 2, 0),
            new Among("alom", -1, 104, 0),
            new Among("ijalom", 1426, 47, 0),
            new Among("nalom", 1426, 46, 0),
            new Among("elom", -1, 83, 0),
            new Among("ilom", -1, 116, 0),
            new Among("ozilom", 1430, 48, 0),
            new Among("olom", -1, 50, 0),
            new Among("ramom", -1, 52, 0),
            new Among("lemom", -1, 51, 0),
            new Among("nom", -1, 13, 0),
            new Among("anom", 1435, 10, 0),
            new Among("inom", 1435, 11, 0),
            new Among("cinom", 1437, 137, 0),
            new Among("aninom", 1437, 10, 0),
            new Among("činom", 1437, 89, 0),
            new Among("onom", 1435, 12, 0),
            new Among("arom", -1, 53, 0),
            new Among("drom", -1, 54, 0),
            new Among("erom", -1, 55, 0),
            new Among("orom", -1, 56, 0),
            new Among("basom", -1, 135, 0),
            new Among("gasom", -1, 131, 0),
            new Among("jasom", -1, 129, 0),
            new Among("kasom", -1, 133, 0),
            new Among("nasom", -1, 132, 0),
            new Among("tasom", -1, 130, 0),
            new Among("vasom", -1, 134, 0),
            new Among("esom", -1, 57, 0),
            new Among("isom", -1, 58, 0),
            new Among("osom", -1, 123, 0),
            new Among("atom", -1, 120, 0),
            new Among("ikatom", 1456, 68, 0),
            new Among("latom", 1456, 69, 0),
            new Among("etom", -1, 70, 0),
            new Among("evitom", -1, 92, 0),
            new Among("ovitom", -1, 93, 0),
            new Among("astom", -1, 94, 0),
            new Among("estom", -1, 71, 0),
            new Among("istom", -1, 72, 0),
            new Among("kstom", -1, 73, 0),
            new Among("ostom", -1, 74, 0),
            new Among("avom", -1, 77, 0),
            new Among("evom", -1, 78, 0),
            new Among("ivom", -1, 79, 0),
            new Among("ovom", -1, 80, 0),
            new Among("lovom", 1470, 82, 0),
            new Among("movom", 1470, 81, 0),
            new Among("stvom", -1, 3, 0),
            new Among("štvom", -1, 4, 0),
            new Among("aćom", -1, 14, 0),
            new Among("ećom", -1, 15, 0),
            new Among("ućom", -1, 16, 0),
            new Among("bašom", -1, 63, 0),
            new Among("gašom", -1, 64, 0),
            new Among("jašom", -1, 61, 0),
            new Among("kašom", -1, 62, 0),
            new Among("našom", -1, 60, 0),
            new Among("tašom", -1, 59, 0),
            new Among("vašom", -1, 65, 0),
            new Among("ešom", -1, 66, 0),
            new Among("išom", -1, 67, 0),
            new Among("ošom", -1, 91, 0),
            new Among("an", -1, 104, 0),
            new Among("acan", 1488, 128, 0),
            new Among("iran", 1488, 100, 0),
            new Among("uran", 1488, 105, 0),
            new Among("tan", 1488, 113, 0),
            new Among("avan", 1488, 97, 0),
            new Among("evan", 1488, 96, 0),
            new Among("ivan", 1488, 98, 0),
            new Among("uvan", 1488, 99, 0),
            new Among("ačan", 1488, 102, 0),
            new Among("acen", -1, 124, 0),
            new Among("lucen", -1, 121, 0),
            new Among("ačen", -1, 101, 0),
            new Among("lučen", -1, 117, 0),
            new Among("anin", -1, 10, 0),
            new Among("ao", -1, 104, 0),
            new Among("acao", 1503, 128, 0),
            new Among("astajao", 1503, 106, 0),
            new Among("istajao", 1503, 107, 0),
            new Among("ostajao", 1503, 108, 0),
            new Among("injao", 1503, 114, 0),
            new Among("irao", 1503, 100, 0),
            new Among("urao", 1503, 105, 0),
            new Among("tao", 1503, 113, 0),
            new Among("astao", 1511, 110, 0),
            new Among("istao", 1511, 111, 0),
            new Among("ostao", 1511, 112, 0),
            new Among("avao", 1503, 97, 0),
            new Among("evao", 1503, 96, 0),
            new Among("ivao", 1503, 98, 0),
            new Among("ovao", 1503, 76, 0),
            new Among("uvao", 1503, 99, 0),
            new Among("ačao", 1503, 102, 0),
            new Among("go", -1, 20, 0),
            new Among("ugo", 1521, 18, 0),
            new Among("io", -1, 116, 0),
            new Among("acio", 1523, 124, 0),
            new Among("lucio", 1523, 121, 0),
            new Among("lio", 1523, 24, 0),
            new Among("nio", 1523, 103, 0),
            new Among("rario", 1523, 21, 0),
            new Among("sio", 1523, 23, 0),
            new Among("rosio", 1529, 127, 0),
            new Among("jetio", 1523, 118, 0),
            new Among("otio", 1523, 22, 0),
            new Among("ačio", 1523, 101, 0),
            new Among("lučio", 1523, 117, 0),
            new Among("rošio", 1523, 90, 0),
            new Among("bijo", -1, 32, 0),
            new Among("cijo", -1, 33, 0),
            new Among("dijo", -1, 34, 0),
            new Among("fijo", -1, 40, 0),
            new Among("gijo", -1, 39, 0),
            new Among("lijo", -1, 35, 0),
            new Among("mijo", -1, 37, 0),
            new Among("nijo", -1, 36, 0),
            new Among("pijo", -1, 41, 0),
            new Among("rijo", -1, 42, 0),
            new Among("sijo", -1, 43, 0),
            new Among("tijo", -1, 44, 0),
            new Among("zijo", -1, 45, 0),
            new Among("žijo", -1, 38, 0),
            new Among("anjo", -1, 84, 0),
            new Among("enjo", -1, 85, 0),
            new Among("snjo", -1, 122, 0),
            new Among("šnjo", -1, 86, 0),
            new Among("ko", -1, 95, 0),
            new Among("sko", 1554, 1, 0),
            new Among("ško", 1554, 2, 0),
            new Among("alo", -1, 104, 0),
            new Among("acalo", 1557, 128, 0),
            new Among("astajalo", 1557, 106, 0),
            new Among("istajalo", 1557, 107, 0),
            new Among("ostajalo", 1557, 108, 0),
            new Among("ijalo", 1557, 47, 0),
            new Among("injalo", 1557, 114, 0),
            new Among("nalo", 1557, 46, 0),
            new Among("iralo", 1557, 100, 0),
            new Among("uralo", 1557, 105, 0),
            new Among("talo", 1557, 113, 0),
            new Among("astalo", 1567, 110, 0),
            new Among("istalo", 1567, 111, 0),
            new Among("ostalo", 1567, 112, 0),
            new Among("avalo", 1557, 97, 0),
            new Among("evalo", 1557, 96, 0),
            new Among("ivalo", 1557, 98, 0),
            new Among("ovalo", 1557, 76, 0),
            new Among("uvalo", 1557, 99, 0),
            new Among("ačalo", 1557, 102, 0),
            new Among("elo", -1, 83, 0),
            new Among("ilo", -1, 116, 0),
            new Among("acilo", 1578, 124, 0),
            new Among("lucilo", 1578, 121, 0),
            new Among("nilo", 1578, 103, 0),
            new Among("rosilo", 1578, 127, 0),
            new Among("jetilo", 1578, 118, 0),
            new Among("ačilo", 1578, 101, 0),
            new Among("lučilo", 1578, 117, 0),
            new Among("rošilo", 1578, 90, 0),
            new Among("aslo", -1, 115, 0),
            new Among("nulo", -1, 13, 0),
            new Among("amo", -1, 104, 0),
            new Among("acamo", 1589, 128, 0),
            new Among("ramo", 1589, 52, 0),
            new Among("iramo", 1591, 100, 0),
            new Among("uramo", 1591, 105, 0),
            new Among("tamo", 1589, 113, 0),
            new Among("avamo", 1589, 97, 0),
            new Among("evamo", 1589, 96, 0),
            new Among("ivamo", 1589, 98, 0),
            new Among("uvamo", 1589, 99, 0),
            new Among("ačamo", 1589, 102, 0),
            new Among("emo", -1, 119, 0),
            new Among("astademo", 1600, 110, 0),
            new Among("istademo", 1600, 111, 0),
            new Among("ostademo", 1600, 112, 0),
            new Among("astajemo", 1600, 106, 0),
            new Among("istajemo", 1600, 107, 0),
            new Among("ostajemo", 1600, 108, 0),
            new Among("ijemo", 1600, 116, 0),
            new Among("injemo", 1600, 114, 0),
            new Among("ujemo", 1600, 25, 0),
            new Among("lucujemo", 1609, 121, 0),
            new Among("irujemo", 1609, 100, 0),
            new Among("lučujemo", 1609, 117, 0),
            new Among("lemo", 1600, 51, 0),
            new Among("nemo", 1600, 13, 0),
            new Among("astanemo", 1614, 110, 0),
            new Among("istanemo", 1614, 111, 0),
            new Among("ostanemo", 1614, 112, 0),
            new Among("etemo", 1600, 70, 0),
            new Among("astemo", 1600, 115, 0),
            new Among("imo", -1, 116, 0),
            new Among("acimo", 1620, 124, 0),
            new Among("lucimo", 1620, 121, 0),
            new Among("nimo", 1620, 13, 0),
            new Among("astanimo", 1623, 110, 0),
            new Among("istanimo", 1623, 111, 0),
            new Among("ostanimo", 1623, 112, 0),
            new Among("rosimo", 1620, 127, 0),
            new Among("etimo", 1620, 70, 0),
            new Among("jetimo", 1628, 118, 0),
            new Among("astimo", 1620, 115, 0),
            new Among("ačimo", 1620, 101, 0),
            new Among("lučimo", 1620, 117, 0),
            new Among("rošimo", 1620, 90, 0),
            new Among("ajmo", -1, 104, 0),
            new Among("urajmo", 1634, 105, 0),
            new Among("tajmo", 1634, 113, 0),
            new Among("astajmo", 1636, 106, 0),
            new Among("istajmo", 1636, 107, 0),
            new Among("ostajmo", 1636, 108, 0),
            new Among("avajmo", 1634, 97, 0),
            new Among("evajmo", 1634, 96, 0),
            new Among("ivajmo", 1634, 98, 0),
            new Among("uvajmo", 1634, 99, 0),
            new Among("ijmo", -1, 116, 0),
            new Among("ujmo", -1, 25, 0),
            new Among("lucujmo", 1645, 121, 0),
            new Among("irujmo", 1645, 100, 0),
            new Among("lučujmo", 1645, 117, 0),
            new Among("asmo", -1, 104, 0),
            new Among("acasmo", 1649, 128, 0),
            new Among("astajasmo", 1649, 106, 0),
            new Among("istajasmo", 1649, 107, 0),
            new Among("ostajasmo", 1649, 108, 0),
            new Among("injasmo", 1649, 114, 0),
            new Among("irasmo", 1649, 100, 0),
            new Among("urasmo", 1649, 105, 0),
            new Among("tasmo", 1649, 113, 0),
            new Among("avasmo", 1649, 97, 0),
            new Among("evasmo", 1649, 96, 0),
            new Among("ivasmo", 1649, 98, 0),
            new Among("ovasmo", 1649, 76, 0),
            new Among("uvasmo", 1649, 99, 0),
            new Among("ačasmo", 1649, 102, 0),
            new Among("ismo", -1, 116, 0),
            new Among("acismo", 1664, 124, 0),
            new Among("lucismo", 1664, 121, 0),
            new Among("nismo", 1664, 103, 0),
            new Among("rosismo", 1664, 127, 0),
            new Among("jetismo", 1664, 118, 0),
            new Among("ačismo", 1664, 101, 0),
            new Among("lučismo", 1664, 117, 0),
            new Among("rošismo", 1664, 90, 0),
            new Among("astadosmo", -1, 110, 0),
            new Among("istadosmo", -1, 111, 0),
            new Among("ostadosmo", -1, 112, 0),
            new Among("nusmo", -1, 13, 0),
            new Among("no", -1, 13, 0),
            new Among("ano", 1677, 104, 0),
            new Among("acano", 1678, 128, 0),
            new Among("urano", 1678, 105, 0),
            new Among("tano", 1678, 113, 0),
            new Among("avano", 1678, 97, 0),
            new Among("evano", 1678, 96, 0),
            new Among("ivano", 1678, 98, 0),
            new Among("uvano", 1678, 99, 0),
            new Among("ačano", 1678, 102, 0),
            new Among("aceno", 1677, 124, 0),
            new Among("luceno", 1677, 121, 0),
            new Among("ačeno", 1677, 101, 0),
            new Among("lučeno", 1677, 117, 0),
            new Among("ino", 1677, 11, 0),
            new Among("cino", 1691, 137, 0),
            new Among("čino", 1691, 89, 0),
            new Among("ato", -1, 120, 0),
            new Among("ikato", 1694, 68, 0),
            new Among("lato", 1694, 69, 0),
            new Among("eto", -1, 70, 0),
            new Among("evito", -1, 92, 0),
            new Among("ovito", -1, 93, 0),
            new Among("asto", -1, 94, 0),
            new Among("esto", -1, 71, 0),
            new Among("isto", -1, 72, 0),
            new Among("ksto", -1, 73, 0),
            new Among("osto", -1, 74, 0),
            new Among("nuto", -1, 13, 0),
            new Among("nuo", -1, 13, 0),
            new Among("avo", -1, 77, 0),
            new Among("evo", -1, 78, 0),
            new Among("ivo", -1, 79, 0),
            new Among("ovo", -1, 80, 0),
            new Among("stvo", -1, 3, 0),
            new Among("štvo", -1, 4, 0),
            new Among("as", -1, 161, 0),
            new Among("acas", 1713, 128, 0),
            new Among("iras", 1713, 155, 0),
            new Among("uras", 1713, 156, 0),
            new Among("tas", 1713, 160, 0),
            new Among("avas", 1713, 144, 0),
            new Among("evas", 1713, 145, 0),
            new Among("ivas", 1713, 146, 0),
            new Among("uvas", 1713, 147, 0),
            new Among("es", -1, 163, 0),
            new Among("astades", 1722, 141, 0),
            new Among("istades", 1722, 142, 0),
            new Among("ostades", 1722, 143, 0),
            new Among("astajes", 1722, 138, 0),
            new Among("istajes", 1722, 139, 0),
            new Among("ostajes", 1722, 140, 0),
            new Among("ijes", 1722, 162, 0),
            new Among("injes", 1722, 150, 0),
            new Among("ujes", 1722, 157, 0),
            new Among("lucujes", 1731, 121, 0),
            new Among("irujes", 1731, 155, 0),
            new Among("nes", 1722, 164, 0),
            new Among("astanes", 1734, 141, 0),
            new Among("istanes", 1734, 142, 0),
            new Among("ostanes", 1734, 143, 0),
            new Among("etes", 1722, 153, 0),
            new Among("astes", 1722, 136, 0),
            new Among("is", -1, 162, 0),
            new Among("acis", 1740, 124, 0),
            new Among("lucis", 1740, 121, 0),
            new Among("nis", 1740, 158, 0),
            new Among("rosis", 1740, 127, 0),
            new Among("jetis", 1740, 149, 0),
            new Among("at", -1, 104, 0),
            new Among("acat", 1746, 128, 0),
            new Among("astajat", 1746, 106, 0),
            new Among("istajat", 1746, 107, 0),
            new Among("ostajat", 1746, 108, 0),
            new Among("injat", 1746, 114, 0),
            new Among("irat", 1746, 100, 0),
            new Among("urat", 1746, 105, 0),
            new Among("tat", 1746, 113, 0),
            new Among("astat", 1754, 110, 0),
            new Among("istat", 1754, 111, 0),
            new Among("ostat", 1754, 112, 0),
            new Among("avat", 1746, 97, 0),
            new Among("evat", 1746, 96, 0),
            new Among("ivat", 1746, 98, 0),
            new Among("irivat", 1760, 100, 0),
            new Among("ovat", 1746, 76, 0),
            new Among("uvat", 1746, 99, 0),
            new Among("ačat", 1746, 102, 0),
            new Among("it", -1, 116, 0),
            new Among("acit", 1765, 124, 0),
            new Among("lucit", 1765, 121, 0),
            new Among("rosit", 1765, 127, 0),
            new Among("jetit", 1765, 118, 0),
            new Among("ačit", 1765, 101, 0),
            new Among("lučit", 1765, 117, 0),
            new Among("rošit", 1765, 90, 0),
            new Among("nut", -1, 13, 0),
            new Among("astadu", -1, 110, 0),
            new Among("istadu", -1, 111, 0),
            new Among("ostadu", -1, 112, 0),
            new Among("gu", -1, 20, 0),
            new Among("logu", 1777, 19, 0),
            new Among("ugu", 1777, 18, 0),
            new Among("ahu", -1, 104, 0),
            new Among("acahu", 1780, 128, 0),
            new Among("astajahu", 1780, 106, 0),
            new Among("istajahu", 1780, 107, 0),
            new Among("ostajahu", 1780, 108, 0),
            new Among("injahu", 1780, 114, 0),
            new Among("irahu", 1780, 100, 0),
            new Among("urahu", 1780, 105, 0),
            new Among("avahu", 1780, 97, 0),
            new Among("evahu", 1780, 96, 0),
            new Among("ivahu", 1780, 98, 0),
            new Among("ovahu", 1780, 76, 0),
            new Among("uvahu", 1780, 99, 0),
            new Among("ačahu", 1780, 102, 0),
            new Among("aju", -1, 104, 0),
            new Among("caju", 1794, 26, 0),
            new Among("acaju", 1795, 128, 0),
            new Among("laju", 1794, 30, 0),
            new Among("raju", 1794, 31, 0),
            new Among("iraju", 1798, 100, 0),
            new Among("uraju", 1798, 105, 0),
            new Among("taju", 1794, 113, 0),
            new Among("astaju", 1801, 106, 0),
            new Among("istaju", 1801, 107, 0),
            new Among("ostaju", 1801, 108, 0),
            new Among("avaju", 1794, 97, 0),
            new Among("evaju", 1794, 96, 0),
            new Among("ivaju", 1794, 98, 0),
            new Among("uvaju", 1794, 99, 0),
            new Among("ćaju", 1794, 28, 0),
            new Among("čaju", 1794, 27, 0),
            new Among("ačaju", 1810, 102, 0),
            new Among("đaju", 1794, 29, 0),
            new Among("iju", -1, 116, 0),
            new Among("biju", 1813, 32, 0),
            new Among("ciju", 1813, 33, 0),
            new Among("diju", 1813, 34, 0),
            new Among("fiju", 1813, 40, 0),
            new Among("giju", 1813, 39, 0),
            new Among("anjiju", 1813, 84, 0),
            new Among("enjiju", 1813, 85, 0),
            new Among("snjiju", 1813, 122, 0),
            new Among("šnjiju", 1813, 86, 0),
            new Among("kiju", 1813, 95, 0),
            new Among("liju", 1813, 24, 0),
            new Among("eliju", 1824, 83, 0),
            new Among("miju", 1813, 37, 0),
            new Among("niju", 1813, 13, 0),
            new Among("ganiju", 1827, 9, 0),
            new Among("maniju", 1827, 6, 0),
            new Among("paniju", 1827, 7, 0),
            new Among("raniju", 1827, 8, 0),
            new Among("taniju", 1827, 5, 0),
            new Among("piju", 1813, 41, 0),
            new Among("riju", 1813, 42, 0),
            new Among("rariju", 1834, 21, 0),
            new Among("siju", 1813, 23, 0),
            new Among("osiju", 1836, 123, 0),
            new Among("tiju", 1813, 44, 0),
            new Among("atiju", 1838, 120, 0),
            new Among("otiju", 1838, 22, 0),
            new Among("aviju", 1813, 77, 0),
            new Among("eviju", 1813, 78, 0),
            new Among("iviju", 1813, 79, 0),
            new Among("oviju", 1813, 80, 0),
            new Among("ziju", 1813, 45, 0),
            new Among("ošiju", 1813, 91, 0),
            new Among("žiju", 1813, 38, 0),
            new Among("anju", -1, 84, 0),
            new Among("enju", -1, 85, 0),
            new Among("snju", -1, 122, 0),
            new Among("šnju", -1, 86, 0),
            new Among("uju", -1, 25, 0),
            new Among("lucuju", 1852, 121, 0),
            new Among("iruju", 1852, 100, 0),
            new Among("lučuju", 1852, 117, 0),
            new Among("ku", -1, 95, 0),
            new Among("sku", 1856, 1, 0),
            new Among("šku", 1856, 2, 0),
            new Among("alu", -1, 104, 0),
            new Among("ijalu", 1859, 47, 0),
            new Among("nalu", 1859, 46, 0),
            new Among("elu", -1, 83, 0),
            new Among("ilu", -1, 116, 0),
            new Among("ozilu", 1863, 48, 0),
            new Among("olu", -1, 50, 0),
            new Among("ramu", -1, 52, 0),
            new Among("acemu", -1, 124, 0),
            new Among("ecemu", -1, 125, 0),
            new Among("ucemu", -1, 126, 0),
            new Among("anjijemu", -1, 84, 0),
            new Among("enjijemu", -1, 85, 0),
            new Among("snjijemu", -1, 122, 0),
            new Among("šnjijemu", -1, 86, 0),
            new Among("kijemu", -1, 95, 0),
            new Among("skijemu", 1874, 1, 0),
            new Among("škijemu", 1874, 2, 0),
            new Among("elijemu", -1, 83, 0),
            new Among("nijemu", -1, 13, 0),
            new Among("osijemu", -1, 123, 0),
            new Among("atijemu", -1, 120, 0),
            new Among("evitijemu", -1, 92, 0),
            new Among("ovitijemu", -1, 93, 0),
            new Among("astijemu", -1, 94, 0),
            new Among("avijemu", -1, 77, 0),
            new Among("evijemu", -1, 78, 0),
            new Among("ivijemu", -1, 79, 0),
            new Among("ovijemu", -1, 80, 0),
            new Among("ošijemu", -1, 91, 0),
            new Among("anjemu", -1, 84, 0),
            new Among("enjemu", -1, 85, 0),
            new Among("snjemu", -1, 122, 0),
            new Among("šnjemu", -1, 86, 0),
            new Among("kemu", -1, 95, 0),
            new Among("skemu", 1893, 1, 0),
            new Among("škemu", 1893, 2, 0),
            new Among("lemu", -1, 51, 0),
            new Among("elemu", 1896, 83, 0),
            new Among("nemu", -1, 13, 0),
            new Among("anemu", 1898, 10, 0),
            new Among("enemu", 1898, 87, 0),
            new Among("snemu", 1898, 159, 0),
            new Among("šnemu", 1898, 88, 0),
            new Among("osemu", -1, 123, 0),
            new Among("atemu", -1, 120, 0),
            new Among("evitemu", -1, 92, 0),
            new Among("ovitemu", -1, 93, 0),
            new Among("astemu", -1, 94, 0),
            new Among("avemu", -1, 77, 0),
            new Among("evemu", -1, 78, 0),
            new Among("ivemu", -1, 79, 0),
            new Among("ovemu", -1, 80, 0),
            new Among("aćemu", -1, 14, 0),
            new Among("ećemu", -1, 15, 0),
            new Among("ućemu", -1, 16, 0),
            new Among("ošemu", -1, 91, 0),
            new Among("acomu", -1, 124, 0),
            new Among("ecomu", -1, 125, 0),
            new Among("ucomu", -1, 126, 0),
            new Among("anjomu", -1, 84, 0),
            new Among("enjomu", -1, 85, 0),
            new Among("snjomu", -1, 122, 0),
            new Among("šnjomu", -1, 86, 0),
            new Among("komu", -1, 95, 0),
            new Among("skomu", 1923, 1, 0),
            new Among("škomu", 1923, 2, 0),
            new Among("elomu", -1, 83, 0),
            new Among("nomu", -1, 13, 0),
            new Among("cinomu", 1927, 137, 0),
            new Among("činomu", 1927, 89, 0),
            new Among("osomu", -1, 123, 0),
            new Among("atomu", -1, 120, 0),
            new Among("evitomu", -1, 92, 0),
            new Among("ovitomu", -1, 93, 0),
            new Among("astomu", -1, 94, 0),
            new Among("avomu", -1, 77, 0),
            new Among("evomu", -1, 78, 0),
            new Among("ivomu", -1, 79, 0),
            new Among("ovomu", -1, 80, 0),
            new Among("aćomu", -1, 14, 0),
            new Among("ećomu", -1, 15, 0),
            new Among("ućomu", -1, 16, 0),
            new Among("ošomu", -1, 91, 0),
            new Among("nu", -1, 13, 0),
            new Among("anu", 1943, 10, 0),
            new Among("astanu", 1944, 110, 0),
            new Among("istanu", 1944, 111, 0),
            new Among("ostanu", 1944, 112, 0),
            new Among("inu", 1943, 11, 0),
            new Among("cinu", 1948, 137, 0),
            new Among("aninu", 1948, 10, 0),
            new Among("činu", 1948, 89, 0),
            new Among("onu", 1943, 12, 0),
            new Among("aru", -1, 53, 0),
            new Among("dru", -1, 54, 0),
            new Among("eru", -1, 55, 0),
            new Among("oru", -1, 56, 0),
            new Among("basu", -1, 135, 0),
            new Among("gasu", -1, 131, 0),
            new Among("jasu", -1, 129, 0),
            new Among("kasu", -1, 133, 0),
            new Among("nasu", -1, 132, 0),
            new Among("tasu", -1, 130, 0),
            new Among("vasu", -1, 134, 0),
            new Among("esu", -1, 57, 0),
            new Among("isu", -1, 58, 0),
            new Among("osu", -1, 123, 0),
            new Among("atu", -1, 120, 0),
            new Among("ikatu", 1967, 68, 0),
            new Among("latu", 1967, 69, 0),
            new Among("etu", -1, 70, 0),
            new Among("evitu", -1, 92, 0),
            new Among("ovitu", -1, 93, 0),
            new Among("astu", -1, 94, 0),
            new Among("estu", -1, 71, 0),
            new Among("istu", -1, 72, 0),
            new Among("kstu", -1, 73, 0),
            new Among("ostu", -1, 74, 0),
            new Among("ištu", -1, 75, 0),
            new Among("avu", -1, 77, 0),
            new Among("evu", -1, 78, 0),
            new Among("ivu", -1, 79, 0),
            new Among("ovu", -1, 80, 0),
            new Among("lovu", 1982, 82, 0),
            new Among("movu", 1982, 81, 0),
            new Among("stvu", -1, 3, 0),
            new Among("štvu", -1, 4, 0),
            new Among("bašu", -1, 63, 0),
            new Among("gašu", -1, 64, 0),
            new Among("jašu", -1, 61, 0),
            new Among("kašu", -1, 62, 0),
            new Among("našu", -1, 60, 0),
            new Among("tašu", -1, 59, 0),
            new Among("vašu", -1, 65, 0),
            new Among("ešu", -1, 66, 0),
            new Among("išu", -1, 67, 0),
            new Among("ošu", -1, 91, 0),
            new Among("avav", -1, 97, 0),
            new Among("evav", -1, 96, 0),
            new Among("ivav", -1, 98, 0),
            new Among("uvav", -1, 99, 0),
            new Among("kov", -1, 95, 0),
            new Among("aš", -1, 104, 0),
            new Among("iraš", 2002, 100, 0),
            new Among("uraš", 2002, 105, 0),
            new Among("taš", 2002, 113, 0),
            new Among("avaš", 2002, 97, 0),
            new Among("evaš", 2002, 96, 0),
            new Among("ivaš", 2002, 98, 0),
            new Among("uvaš", 2002, 99, 0),
            new Among("ačaš", 2002, 102, 0),
            new Among("eš", -1, 119, 0),
            new Among("astadeš", 2011, 110, 0),
            new Among("istadeš", 2011, 111, 0),
            new Among("ostadeš", 2011, 112, 0),
            new Among("astaješ", 2011, 106, 0),
            new Among("istaješ", 2011, 107, 0),
            new Among("ostaješ", 2011, 108, 0),
            new Among("iješ", 2011, 116, 0),
            new Among("inješ", 2011, 114, 0),
            new Among("uješ", 2011, 25, 0),
            new Among("iruješ", 2020, 100, 0),
            new Among("lučuješ", 2020, 117, 0),
            new Among("neš", 2011, 13, 0),
            new Among("astaneš", 2023, 110, 0),
            new Among("istaneš", 2023, 111, 0),
            new Among("ostaneš", 2023, 112, 0),
            new Among("eteš", 2011, 70, 0),
            new Among("asteš", 2011, 115, 0),
            new Among("iš", -1, 116, 0),
            new Among("niš", 2029, 103, 0),
            new Among("jetiš", 2029, 118, 0),
            new Among("ačiš", 2029, 101, 0),
            new Among("lučiš", 2029, 117, 0),
            new Among("rošiš", 2029, 90, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("oga", 0, 1, 0),
            new Among("ama", 0, 1, 0),
            new Among("ima", 0, 1, 0),
            new Among("ena", 0, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("og", -1, 1, 0),
            new Among("anog", 6, 1, 0),
            new Among("enog", 6, 1, 0),
            new Among("anih", -1, 1, 0),
            new Among("enih", -1, 1, 0),
            new Among("i", -1, 1, 0),
            new Among("ani", 11, 1, 0),
            new Among("eni", 11, 1, 0),
            new Among("anoj", -1, 1, 0),
            new Among("enoj", -1, 1, 0),
            new Among("anim", -1, 1, 0),
            new Among("enim", -1, 1, 0),
            new Among("om", -1, 1, 0),
            new Among("enom", 18, 1, 0),
            new Among("o", -1, 1, 0),
            new Among("ano", 20, 1, 0),
            new Among("eno", 20, 1, 0),
            new Among("ost", -1, 1, 0),
            new Among("u", -1, 1, 0),
            new Among("enu", 24, 1, 0)
        };


        private bool r_cyr_to_lat()
        {
            int among_var;
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
                        bra = cursor;
                        among_var = find_among(a_0, null);
                        if (among_var == 0)
                        {
                            goto lab2;
                        }
                        ket = cursor;
                        switch (among_var) {
                            case 1: {
                                slice_from("a");
                                break;
                            }
                            case 2: {
                                slice_from("b");
                                break;
                            }
                            case 3: {
                                slice_from("v");
                                break;
                            }
                            case 4: {
                                slice_from("g");
                                break;
                            }
                            case 5: {
                                slice_from("d");
                                break;
                            }
                            case 6: {
                                slice_from("đ");
                                break;
                            }
                            case 7: {
                                slice_from("e");
                                break;
                            }
                            case 8: {
                                slice_from("ž");
                                break;
                            }
                            case 9: {
                                slice_from("z");
                                break;
                            }
                            case 10: {
                                slice_from("i");
                                break;
                            }
                            case 11: {
                                slice_from("j");
                                break;
                            }
                            case 12: {
                                slice_from("k");
                                break;
                            }
                            case 13: {
                                slice_from("l");
                                break;
                            }
                            case 14: {
                                slice_from("lj");
                                break;
                            }
                            case 15: {
                                slice_from("m");
                                break;
                            }
                            case 16: {
                                slice_from("n");
                                break;
                            }
                            case 17: {
                                slice_from("nj");
                                break;
                            }
                            case 18: {
                                slice_from("o");
                                break;
                            }
                            case 19: {
                                slice_from("p");
                                break;
                            }
                            case 20: {
                                slice_from("r");
                                break;
                            }
                            case 21: {
                                slice_from("s");
                                break;
                            }
                            case 22: {
                                slice_from("t");
                                break;
                            }
                            case 23: {
                                slice_from("ć");
                                break;
                            }
                            case 24: {
                                slice_from("u");
                                break;
                            }
                            case 25: {
                                slice_from("f");
                                break;
                            }
                            case 26: {
                                slice_from("h");
                                break;
                            }
                            case 27: {
                                slice_from("c");
                                break;
                            }
                            case 28: {
                                slice_from("č");
                                break;
                            }
                            case 29: {
                                slice_from("dž");
                                break;
                            }
                            case 30: {
                                slice_from("š");
                                break;
                            }
                        }
                        cursor = c3;
                        break;
                    lab2: ;
                        cursor = c3;
                        if (cursor >= limit)
                        {
                            goto lab1;
                        }
                        cursor++;
                    }
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            return true;
        }

        private bool r_prelude()
        {
            {
                int c1 = cursor;
                while (true)
                {
                    int c2 = cursor;
                    while (true)
                    {
                        int c3 = cursor;
                        if (in_grouping(g_ca, 98, 382, false) != 0)
                        {
                            goto lab2;
                        }
                        bra = cursor;
                        if (!(eq_s("ije")))
                        {
                            goto lab2;
                        }
                        ket = cursor;
                        if (in_grouping(g_ca, 98, 382, false) != 0)
                        {
                            goto lab2;
                        }
                        slice_from("e");
                        cursor = c3;
                        break;
                    lab2: ;
                        cursor = c3;
                        if (cursor >= limit)
                        {
                            goto lab1;
                        }
                        cursor++;
                    }
                    continue;
                lab1: ;
                    cursor = c2;
                    break;
                }
                cursor = c1;
            }
            {
                int c4 = cursor;
                while (true)
                {
                    int c5 = cursor;
                    while (true)
                    {
                        int c6 = cursor;
                        if (in_grouping(g_ca, 98, 382, false) != 0)
                        {
                            goto lab5;
                        }
                        bra = cursor;
                        if (!(eq_s("je")))
                        {
                            goto lab5;
                        }
                        ket = cursor;
                        if (in_grouping(g_ca, 98, 382, false) != 0)
                        {
                            goto lab5;
                        }
                        slice_from("e");
                        cursor = c6;
                        break;
                    lab5: ;
                        cursor = c6;
                        if (cursor >= limit)
                        {
                            goto lab4;
                        }
                        cursor++;
                    }
                    continue;
                lab4: ;
                    cursor = c5;
                    break;
                }
                cursor = c4;
            }
            {
                int c7 = cursor;
                while (true)
                {
                    int c8 = cursor;
                    while (true)
                    {
                        int c9 = cursor;
                        bra = cursor;
                        if (!(eq_s("dj")))
                        {
                            goto lab8;
                        }
                        ket = cursor;
                        slice_from("đ");
                        cursor = c9;
                        break;
                    lab8: ;
                        cursor = c9;
                        if (cursor >= limit)
                        {
                            goto lab7;
                        }
                        cursor++;
                    }
                    continue;
                lab7: ;
                    cursor = c8;
                    break;
                }
                cursor = c7;
            }
            return true;
        }

        private bool r_mark_regions()
        {
            B_no_diacritics = true;
            {
                int c1 = cursor;
                {

                    int ret = out_grouping(g_sa, 263, 382, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                B_no_diacritics = false;
            lab0: ;
                cursor = c1;
            }
            I_p1 = limit;
            {
                int c2 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 117, true);
                    if (ret < 0)
                    {
                        goto lab1;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                if (I_p1 >= 2)
                {
                    goto lab1;
                }
                {

                    int ret = in_grouping(g_v, 97, 117, true);
                    if (ret < 0)
                    {
                        goto lab1;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
            lab1: ;
                cursor = c2;
            }
            {
                int c3 = cursor;
                while (true)
                {
                    if (!(eq_s("r")))
                    {
                        goto lab3;
                    }
                    break;
                lab3: ;
                    if (cursor >= limit)
                    {
                        goto lab2;
                    }
                    cursor++;
                }
                {
                    int c4 = cursor;
                    if (cursor < 2)
                    {
                        goto lab5;
                    }
                    goto lab4;
                lab5: ;
                    cursor = c4;
                    {

                        int ret = in_grouping(g_rg, 114, 114, true);
                        if (ret < 0)
                        {
                            goto lab2;
                        }

                        cursor += ret;
                    }
                }
            lab4: ;
                if ((I_p1 - cursor) <= 1)
                {
                    goto lab2;
                }
                I_p1 = cursor;
            lab2: ;
                cursor = c3;
            }
            return true;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_Step_1()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_1, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    slice_from("loga");
                    break;
                }
                case 2: {
                    slice_from("peh");
                    break;
                }
                case 3: {
                    slice_from("vojka");
                    break;
                }
                case 4: {
                    slice_from("bojka");
                    break;
                }
                case 5: {
                    slice_from("jak");
                    break;
                }
                case 6: {
                    slice_from("čajni");
                    break;
                }
                case 7: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("cajni");
                    break;
                }
                case 8: {
                    slice_from("erni");
                    break;
                }
                case 9: {
                    slice_from("larni");
                    break;
                }
                case 10: {
                    slice_from("esni");
                    break;
                }
                case 11: {
                    slice_from("anjca");
                    break;
                }
                case 12: {
                    slice_from("ajca");
                    break;
                }
                case 13: {
                    slice_from("ljca");
                    break;
                }
                case 14: {
                    slice_from("ejca");
                    break;
                }
                case 15: {
                    slice_from("ojca");
                    break;
                }
                case 16: {
                    slice_from("ajka");
                    break;
                }
                case 17: {
                    slice_from("ojka");
                    break;
                }
                case 18: {
                    slice_from("šca");
                    break;
                }
                case 19: {
                    slice_from("ing");
                    break;
                }
                case 20: {
                    slice_from("tvenik");
                    break;
                }
                case 21: {
                    slice_from("tetika");
                    break;
                }
                case 22: {
                    slice_from("nstva");
                    break;
                }
                case 23: {
                    slice_from("nik");
                    break;
                }
                case 24: {
                    slice_from("tik");
                    break;
                }
                case 25: {
                    slice_from("zik");
                    break;
                }
                case 26: {
                    slice_from("snik");
                    break;
                }
                case 27: {
                    slice_from("kusi");
                    break;
                }
                case 28: {
                    slice_from("kusni");
                    break;
                }
                case 29: {
                    slice_from("kustva");
                    break;
                }
                case 30: {
                    slice_from("dušni");
                    break;
                }
                case 31: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("dusni");
                    break;
                }
                case 32: {
                    slice_from("antni");
                    break;
                }
                case 33: {
                    slice_from("bilni");
                    break;
                }
                case 34: {
                    slice_from("tilni");
                    break;
                }
                case 35: {
                    slice_from("avilni");
                    break;
                }
                case 36: {
                    slice_from("silni");
                    break;
                }
                case 37: {
                    slice_from("gilni");
                    break;
                }
                case 38: {
                    slice_from("rilni");
                    break;
                }
                case 39: {
                    slice_from("nilni");
                    break;
                }
                case 40: {
                    slice_from("alni");
                    break;
                }
                case 41: {
                    slice_from("ozni");
                    break;
                }
                case 42: {
                    slice_from("ravi");
                    break;
                }
                case 43: {
                    slice_from("stavni");
                    break;
                }
                case 44: {
                    slice_from("pravni");
                    break;
                }
                case 45: {
                    slice_from("tivni");
                    break;
                }
                case 46: {
                    slice_from("sivni");
                    break;
                }
                case 47: {
                    slice_from("atni");
                    break;
                }
                case 48: {
                    slice_from("enta");
                    break;
                }
                case 49: {
                    slice_from("tetni");
                    break;
                }
                case 50: {
                    slice_from("pletni");
                    break;
                }
                case 51: {
                    slice_from("šavi");
                    break;
                }
                case 52: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("savi");
                    break;
                }
                case 53: {
                    slice_from("anta");
                    break;
                }
                case 54: {
                    slice_from("ačka");
                    break;
                }
                case 55: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("acka");
                    break;
                }
                case 56: {
                    slice_from("uška");
                    break;
                }
                case 57: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("uska");
                    break;
                }
                case 58: {
                    slice_from("atka");
                    break;
                }
                case 59: {
                    slice_from("etka");
                    break;
                }
                case 60: {
                    slice_from("itka");
                    break;
                }
                case 61: {
                    slice_from("otka");
                    break;
                }
                case 62: {
                    slice_from("utka");
                    break;
                }
                case 63: {
                    slice_from("eskna");
                    break;
                }
                case 64: {
                    slice_from("tični");
                    break;
                }
                case 65: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ticni");
                    break;
                }
                case 66: {
                    slice_from("ojska");
                    break;
                }
                case 67: {
                    slice_from("esma");
                    break;
                }
                case 68: {
                    slice_from("metra");
                    break;
                }
                case 69: {
                    slice_from("centra");
                    break;
                }
                case 70: {
                    slice_from("istra");
                    break;
                }
                case 71: {
                    slice_from("osti");
                    break;
                }
                case 72: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("osti");
                    break;
                }
                case 73: {
                    slice_from("dba");
                    break;
                }
                case 74: {
                    slice_from("čka");
                    break;
                }
                case 75: {
                    slice_from("mca");
                    break;
                }
                case 76: {
                    slice_from("nca");
                    break;
                }
                case 77: {
                    slice_from("voljni");
                    break;
                }
                case 78: {
                    slice_from("anki");
                    break;
                }
                case 79: {
                    slice_from("vca");
                    break;
                }
                case 80: {
                    slice_from("sca");
                    break;
                }
                case 81: {
                    slice_from("rca");
                    break;
                }
                case 82: {
                    slice_from("alca");
                    break;
                }
                case 83: {
                    slice_from("elca");
                    break;
                }
                case 84: {
                    slice_from("olca");
                    break;
                }
                case 85: {
                    slice_from("njca");
                    break;
                }
                case 86: {
                    slice_from("ekta");
                    break;
                }
                case 87: {
                    slice_from("izma");
                    break;
                }
                case 88: {
                    slice_from("jebi");
                    break;
                }
                case 89: {
                    slice_from("baci");
                    break;
                }
                case 90: {
                    slice_from("ašni");
                    break;
                }
                case 91: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("asni");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_2()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            switch (among_var) {
                case 1: {
                    slice_from("sk");
                    break;
                }
                case 2: {
                    slice_from("šk");
                    break;
                }
                case 3: {
                    slice_from("stv");
                    break;
                }
                case 4: {
                    slice_from("štv");
                    break;
                }
                case 5: {
                    slice_from("tanij");
                    break;
                }
                case 6: {
                    slice_from("manij");
                    break;
                }
                case 7: {
                    slice_from("panij");
                    break;
                }
                case 8: {
                    slice_from("ranij");
                    break;
                }
                case 9: {
                    slice_from("ganij");
                    break;
                }
                case 10: {
                    slice_from("an");
                    break;
                }
                case 11: {
                    slice_from("in");
                    break;
                }
                case 12: {
                    slice_from("on");
                    break;
                }
                case 13: {
                    slice_from("n");
                    break;
                }
                case 14: {
                    slice_from("ać");
                    break;
                }
                case 15: {
                    slice_from("eć");
                    break;
                }
                case 16: {
                    slice_from("uć");
                    break;
                }
                case 17: {
                    slice_from("ugov");
                    break;
                }
                case 18: {
                    slice_from("ug");
                    break;
                }
                case 19: {
                    slice_from("log");
                    break;
                }
                case 20: {
                    slice_from("g");
                    break;
                }
                case 21: {
                    slice_from("rari");
                    break;
                }
                case 22: {
                    slice_from("oti");
                    break;
                }
                case 23: {
                    slice_from("si");
                    break;
                }
                case 24: {
                    slice_from("li");
                    break;
                }
                case 25: {
                    slice_from("uj");
                    break;
                }
                case 26: {
                    slice_from("caj");
                    break;
                }
                case 27: {
                    slice_from("čaj");
                    break;
                }
                case 28: {
                    slice_from("ćaj");
                    break;
                }
                case 29: {
                    slice_from("đaj");
                    break;
                }
                case 30: {
                    slice_from("laj");
                    break;
                }
                case 31: {
                    slice_from("raj");
                    break;
                }
                case 32: {
                    slice_from("bij");
                    break;
                }
                case 33: {
                    slice_from("cij");
                    break;
                }
                case 34: {
                    slice_from("dij");
                    break;
                }
                case 35: {
                    slice_from("lij");
                    break;
                }
                case 36: {
                    slice_from("nij");
                    break;
                }
                case 37: {
                    slice_from("mij");
                    break;
                }
                case 38: {
                    slice_from("žij");
                    break;
                }
                case 39: {
                    slice_from("gij");
                    break;
                }
                case 40: {
                    slice_from("fij");
                    break;
                }
                case 41: {
                    slice_from("pij");
                    break;
                }
                case 42: {
                    slice_from("rij");
                    break;
                }
                case 43: {
                    slice_from("sij");
                    break;
                }
                case 44: {
                    slice_from("tij");
                    break;
                }
                case 45: {
                    slice_from("zij");
                    break;
                }
                case 46: {
                    slice_from("nal");
                    break;
                }
                case 47: {
                    slice_from("ijal");
                    break;
                }
                case 48: {
                    slice_from("ozil");
                    break;
                }
                case 49: {
                    slice_from("olov");
                    break;
                }
                case 50: {
                    slice_from("ol");
                    break;
                }
                case 51: {
                    slice_from("lem");
                    break;
                }
                case 52: {
                    slice_from("ram");
                    break;
                }
                case 53: {
                    slice_from("ar");
                    break;
                }
                case 54: {
                    slice_from("dr");
                    break;
                }
                case 55: {
                    slice_from("er");
                    break;
                }
                case 56: {
                    slice_from("or");
                    break;
                }
                case 57: {
                    slice_from("es");
                    break;
                }
                case 58: {
                    slice_from("is");
                    break;
                }
                case 59: {
                    slice_from("taš");
                    break;
                }
                case 60: {
                    slice_from("naš");
                    break;
                }
                case 61: {
                    slice_from("jaš");
                    break;
                }
                case 62: {
                    slice_from("kaš");
                    break;
                }
                case 63: {
                    slice_from("baš");
                    break;
                }
                case 64: {
                    slice_from("gaš");
                    break;
                }
                case 65: {
                    slice_from("vaš");
                    break;
                }
                case 66: {
                    slice_from("eš");
                    break;
                }
                case 67: {
                    slice_from("iš");
                    break;
                }
                case 68: {
                    slice_from("ikat");
                    break;
                }
                case 69: {
                    slice_from("lat");
                    break;
                }
                case 70: {
                    slice_from("et");
                    break;
                }
                case 71: {
                    slice_from("est");
                    break;
                }
                case 72: {
                    slice_from("ist");
                    break;
                }
                case 73: {
                    slice_from("kst");
                    break;
                }
                case 74: {
                    slice_from("ost");
                    break;
                }
                case 75: {
                    slice_from("išt");
                    break;
                }
                case 76: {
                    slice_from("ova");
                    break;
                }
                case 77: {
                    slice_from("av");
                    break;
                }
                case 78: {
                    slice_from("ev");
                    break;
                }
                case 79: {
                    slice_from("iv");
                    break;
                }
                case 80: {
                    slice_from("ov");
                    break;
                }
                case 81: {
                    slice_from("mov");
                    break;
                }
                case 82: {
                    slice_from("lov");
                    break;
                }
                case 83: {
                    slice_from("el");
                    break;
                }
                case 84: {
                    slice_from("anj");
                    break;
                }
                case 85: {
                    slice_from("enj");
                    break;
                }
                case 86: {
                    slice_from("šnj");
                    break;
                }
                case 87: {
                    slice_from("en");
                    break;
                }
                case 88: {
                    slice_from("šn");
                    break;
                }
                case 89: {
                    slice_from("čin");
                    break;
                }
                case 90: {
                    slice_from("roši");
                    break;
                }
                case 91: {
                    slice_from("oš");
                    break;
                }
                case 92: {
                    slice_from("evit");
                    break;
                }
                case 93: {
                    slice_from("ovit");
                    break;
                }
                case 94: {
                    slice_from("ast");
                    break;
                }
                case 95: {
                    slice_from("k");
                    break;
                }
                case 96: {
                    slice_from("eva");
                    break;
                }
                case 97: {
                    slice_from("ava");
                    break;
                }
                case 98: {
                    slice_from("iva");
                    break;
                }
                case 99: {
                    slice_from("uva");
                    break;
                }
                case 100: {
                    slice_from("ir");
                    break;
                }
                case 101: {
                    slice_from("ač");
                    break;
                }
                case 102: {
                    slice_from("ača");
                    break;
                }
                case 103: {
                    slice_from("ni");
                    break;
                }
                case 104: {
                    slice_from("a");
                    break;
                }
                case 105: {
                    slice_from("ur");
                    break;
                }
                case 106: {
                    slice_from("astaj");
                    break;
                }
                case 107: {
                    slice_from("istaj");
                    break;
                }
                case 108: {
                    slice_from("ostaj");
                    break;
                }
                case 109: {
                    slice_from("aj");
                    break;
                }
                case 110: {
                    slice_from("asta");
                    break;
                }
                case 111: {
                    slice_from("ista");
                    break;
                }
                case 112: {
                    slice_from("osta");
                    break;
                }
                case 113: {
                    slice_from("ta");
                    break;
                }
                case 114: {
                    slice_from("inj");
                    break;
                }
                case 115: {
                    slice_from("as");
                    break;
                }
                case 116: {
                    slice_from("i");
                    break;
                }
                case 117: {
                    slice_from("luč");
                    break;
                }
                case 118: {
                    slice_from("jeti");
                    break;
                }
                case 119: {
                    slice_from("e");
                    break;
                }
                case 120: {
                    slice_from("at");
                    break;
                }
                case 121: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("luc");
                    break;
                }
                case 122: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("snj");
                    break;
                }
                case 123: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("os");
                    break;
                }
                case 124: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ac");
                    break;
                }
                case 125: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ec");
                    break;
                }
                case 126: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("uc");
                    break;
                }
                case 127: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("rosi");
                    break;
                }
                case 128: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("aca");
                    break;
                }
                case 129: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("jas");
                    break;
                }
                case 130: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("tas");
                    break;
                }
                case 131: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("gas");
                    break;
                }
                case 132: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("nas");
                    break;
                }
                case 133: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("kas");
                    break;
                }
                case 134: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("vas");
                    break;
                }
                case 135: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("bas");
                    break;
                }
                case 136: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("as");
                    break;
                }
                case 137: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("cin");
                    break;
                }
                case 138: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("astaj");
                    break;
                }
                case 139: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("istaj");
                    break;
                }
                case 140: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ostaj");
                    break;
                }
                case 141: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("asta");
                    break;
                }
                case 142: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ista");
                    break;
                }
                case 143: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("osta");
                    break;
                }
                case 144: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ava");
                    break;
                }
                case 145: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("eva");
                    break;
                }
                case 146: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("iva");
                    break;
                }
                case 147: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("uva");
                    break;
                }
                case 148: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ova");
                    break;
                }
                case 149: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("jeti");
                    break;
                }
                case 150: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("inj");
                    break;
                }
                case 151: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ist");
                    break;
                }
                case 152: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("es");
                    break;
                }
                case 153: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("et");
                    break;
                }
                case 154: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("is");
                    break;
                }
                case 155: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ir");
                    break;
                }
                case 156: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ur");
                    break;
                }
                case 157: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("uj");
                    break;
                }
                case 158: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ni");
                    break;
                }
                case 159: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("sn");
                    break;
                }
                case 160: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("ta");
                    break;
                }
                case 161: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("a");
                    break;
                }
                case 162: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("i");
                    break;
                }
                case 163: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("e");
                    break;
                }
                case 164: {
                    if (!B_no_diacritics)
                    {
                        return false;
                    }
                    slice_from("n");
                    break;
                }
            }
            return true;
        }

        private bool r_Step_3()
        {
            ket = cursor;
            if (find_among_b(a_3, null) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            slice_from("");
            return true;
        }

        protected override bool stem()
        {
            r_cyr_to_lat();
            r_prelude();
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                r_Step_1();
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                {
                    int c3 = limit - cursor;
                    if (!r_Step_2())
                        goto lab2;
                    goto lab1;
                lab2: ;
                    cursor = limit - c3;
                    if (!r_Step_3())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c2;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

