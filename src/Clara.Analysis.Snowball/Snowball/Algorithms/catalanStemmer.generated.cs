// Generated from catalan.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from catalan.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class CatalanStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;

        private const string g_v = "aeiouàáèéíïòóúü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 7, 0),
            new Among("·", 0, 6, 0),
            new Among("à", 0, 1, 0),
            new Among("á", 0, 1, 0),
            new Among("è", 0, 2, 0),
            new Among("é", 0, 2, 0),
            new Among("ì", 0, 3, 0),
            new Among("í", 0, 3, 0),
            new Among("ï", 0, 3, 0),
            new Among("ò", 0, 4, 0),
            new Among("ó", 0, 4, 0),
            new Among("ú", 0, 5, 0),
            new Among("ü", 0, 5, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("la", -1, 1, 0),
            new Among("-la", 0, 1, 0),
            new Among("sela", 0, 1, 0),
            new Among("le", -1, 1, 0),
            new Among("me", -1, 1, 0),
            new Among("-me", 4, 1, 0),
            new Among("se", -1, 1, 0),
            new Among("-te", -1, 1, 0),
            new Among("hi", -1, 1, 0),
            new Among("'hi", 8, 1, 0),
            new Among("li", -1, 1, 0),
            new Among("-li", 10, 1, 0),
            new Among("'l", -1, 1, 0),
            new Among("'m", -1, 1, 0),
            new Among("-m", -1, 1, 0),
            new Among("'n", -1, 1, 0),
            new Among("-n", -1, 1, 0),
            new Among("ho", -1, 1, 0),
            new Among("'ho", 17, 1, 0),
            new Among("lo", -1, 1, 0),
            new Among("selo", 19, 1, 0),
            new Among("'s", -1, 1, 0),
            new Among("las", -1, 1, 0),
            new Among("selas", 22, 1, 0),
            new Among("les", -1, 1, 0),
            new Among("-les", 24, 1, 0),
            new Among("'ls", -1, 1, 0),
            new Among("-ls", -1, 1, 0),
            new Among("'ns", -1, 1, 0),
            new Among("-ns", -1, 1, 0),
            new Among("ens", -1, 1, 0),
            new Among("los", -1, 1, 0),
            new Among("selos", 31, 1, 0),
            new Among("nos", -1, 1, 0),
            new Among("-nos", 33, 1, 0),
            new Among("vos", -1, 1, 0),
            new Among("us", -1, 1, 0),
            new Among("-us", 36, 1, 0),
            new Among("'t", -1, 1, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ica", -1, 4, 0),
            new Among("lógica", 0, 3, 0),
            new Among("enca", -1, 1, 0),
            new Among("ada", -1, 2, 0),
            new Among("ancia", -1, 1, 0),
            new Among("encia", -1, 1, 0),
            new Among("ència", -1, 1, 0),
            new Among("ícia", -1, 1, 0),
            new Among("logia", -1, 3, 0),
            new Among("inia", -1, 1, 0),
            new Among("íinia", 9, 1, 0),
            new Among("eria", -1, 1, 0),
            new Among("ària", -1, 1, 0),
            new Among("atòria", -1, 1, 0),
            new Among("alla", -1, 1, 0),
            new Among("ella", -1, 1, 0),
            new Among("ívola", -1, 1, 0),
            new Among("ima", -1, 1, 0),
            new Among("íssima", 17, 1, 0),
            new Among("quíssima", 18, 5, 0),
            new Among("ana", -1, 1, 0),
            new Among("ina", -1, 1, 0),
            new Among("era", -1, 1, 0),
            new Among("sfera", 22, 1, 0),
            new Among("ora", -1, 1, 0),
            new Among("dora", 24, 1, 0),
            new Among("adora", 25, 1, 0),
            new Among("adura", -1, 1, 0),
            new Among("esa", -1, 1, 0),
            new Among("osa", -1, 1, 0),
            new Among("assa", -1, 1, 0),
            new Among("essa", -1, 1, 0),
            new Among("issa", -1, 1, 0),
            new Among("eta", -1, 1, 0),
            new Among("ita", -1, 1, 0),
            new Among("ota", -1, 1, 0),
            new Among("ista", -1, 1, 0),
            new Among("ialista", 36, 1, 0),
            new Among("ionista", 36, 1, 0),
            new Among("iva", -1, 1, 0),
            new Among("ativa", 39, 1, 0),
            new Among("nça", -1, 1, 0),
            new Among("logía", -1, 3, 0),
            new Among("ic", -1, 4, 0),
            new Among("ístic", 43, 1, 0),
            new Among("enc", -1, 1, 0),
            new Among("esc", -1, 1, 0),
            new Among("ud", -1, 1, 0),
            new Among("atge", -1, 1, 0),
            new Among("ble", -1, 1, 0),
            new Among("able", 49, 1, 0),
            new Among("ible", 49, 1, 0),
            new Among("isme", -1, 1, 0),
            new Among("ialisme", 52, 1, 0),
            new Among("ionisme", 52, 1, 0),
            new Among("ivisme", 52, 1, 0),
            new Among("aire", -1, 1, 0),
            new Among("icte", -1, 1, 0),
            new Among("iste", -1, 1, 0),
            new Among("ici", -1, 1, 0),
            new Among("íci", -1, 1, 0),
            new Among("logi", -1, 3, 0),
            new Among("ari", -1, 1, 0),
            new Among("tori", -1, 1, 0),
            new Among("al", -1, 1, 0),
            new Among("il", -1, 1, 0),
            new Among("all", -1, 1, 0),
            new Among("ell", -1, 1, 0),
            new Among("ívol", -1, 1, 0),
            new Among("isam", -1, 1, 0),
            new Among("issem", -1, 1, 0),
            new Among("ìssem", -1, 1, 0),
            new Among("íssem", -1, 1, 0),
            new Among("íssim", -1, 1, 0),
            new Among("quíssim", 73, 5, 0),
            new Among("amen", -1, 1, 0),
            new Among("ìssin", -1, 1, 0),
            new Among("ar", -1, 1, 0),
            new Among("ificar", 77, 1, 0),
            new Among("egar", 77, 1, 0),
            new Among("ejar", 77, 1, 0),
            new Among("itar", 77, 1, 0),
            new Among("itzar", 77, 1, 0),
            new Among("fer", -1, 1, 0),
            new Among("or", -1, 1, 0),
            new Among("dor", 84, 1, 0),
            new Among("dur", -1, 1, 0),
            new Among("doras", -1, 1, 0),
            new Among("ics", -1, 4, 0),
            new Among("lógics", 88, 3, 0),
            new Among("uds", -1, 1, 0),
            new Among("nces", -1, 1, 0),
            new Among("ades", -1, 2, 0),
            new Among("ancies", -1, 1, 0),
            new Among("encies", -1, 1, 0),
            new Among("ències", -1, 1, 0),
            new Among("ícies", -1, 1, 0),
            new Among("logies", -1, 3, 0),
            new Among("inies", -1, 1, 0),
            new Among("ínies", -1, 1, 0),
            new Among("eries", -1, 1, 0),
            new Among("àries", -1, 1, 0),
            new Among("atòries", -1, 1, 0),
            new Among("bles", -1, 1, 0),
            new Among("ables", 103, 1, 0),
            new Among("ibles", 103, 1, 0),
            new Among("imes", -1, 1, 0),
            new Among("íssimes", 106, 1, 0),
            new Among("quíssimes", 107, 5, 0),
            new Among("formes", -1, 1, 0),
            new Among("ismes", -1, 1, 0),
            new Among("ialismes", 110, 1, 0),
            new Among("ines", -1, 1, 0),
            new Among("eres", -1, 1, 0),
            new Among("ores", -1, 1, 0),
            new Among("dores", 114, 1, 0),
            new Among("idores", 115, 1, 0),
            new Among("dures", -1, 1, 0),
            new Among("eses", -1, 1, 0),
            new Among("oses", -1, 1, 0),
            new Among("asses", -1, 1, 0),
            new Among("ictes", -1, 1, 0),
            new Among("ites", -1, 1, 0),
            new Among("otes", -1, 1, 0),
            new Among("istes", -1, 1, 0),
            new Among("ialistes", 124, 1, 0),
            new Among("ionistes", 124, 1, 0),
            new Among("iques", -1, 4, 0),
            new Among("lógiques", 127, 3, 0),
            new Among("ives", -1, 1, 0),
            new Among("atives", 129, 1, 0),
            new Among("logíes", -1, 3, 0),
            new Among("allengües", -1, 1, 0),
            new Among("icis", -1, 1, 0),
            new Among("ícis", -1, 1, 0),
            new Among("logis", -1, 3, 0),
            new Among("aris", -1, 1, 0),
            new Among("toris", -1, 1, 0),
            new Among("ls", -1, 1, 0),
            new Among("als", 138, 1, 0),
            new Among("ells", 138, 1, 0),
            new Among("ims", -1, 1, 0),
            new Among("íssims", 141, 1, 0),
            new Among("quíssims", 142, 5, 0),
            new Among("ions", -1, 1, 0),
            new Among("cions", 144, 1, 0),
            new Among("acions", 145, 2, 0),
            new Among("esos", -1, 1, 0),
            new Among("osos", -1, 1, 0),
            new Among("assos", -1, 1, 0),
            new Among("issos", -1, 1, 0),
            new Among("ers", -1, 1, 0),
            new Among("ors", -1, 1, 0),
            new Among("dors", 152, 1, 0),
            new Among("adors", 153, 1, 0),
            new Among("idors", 153, 1, 0),
            new Among("ats", -1, 1, 0),
            new Among("itats", 156, 1, 0),
            new Among("bilitats", 157, 1, 0),
            new Among("ivitats", 157, 1, 0),
            new Among("ativitats", 159, 1, 0),
            new Among("ïtats", 156, 1, 0),
            new Among("ets", -1, 1, 0),
            new Among("ants", -1, 1, 0),
            new Among("ents", -1, 1, 0),
            new Among("ments", 164, 1, 0),
            new Among("aments", 165, 1, 0),
            new Among("ots", -1, 1, 0),
            new Among("uts", -1, 1, 0),
            new Among("ius", -1, 1, 0),
            new Among("trius", 169, 1, 0),
            new Among("atius", 169, 1, 0),
            new Among("ès", -1, 1, 0),
            new Among("és", -1, 1, 0),
            new Among("ís", -1, 1, 0),
            new Among("dís", 174, 1, 0),
            new Among("ós", -1, 1, 0),
            new Among("itat", -1, 1, 0),
            new Among("bilitat", 177, 1, 0),
            new Among("ivitat", 177, 1, 0),
            new Among("ativitat", 179, 1, 0),
            new Among("ïtat", -1, 1, 0),
            new Among("et", -1, 1, 0),
            new Among("ant", -1, 1, 0),
            new Among("ent", -1, 1, 0),
            new Among("ient", 184, 1, 0),
            new Among("ment", 184, 1, 0),
            new Among("ament", 186, 1, 0),
            new Among("isament", 187, 1, 0),
            new Among("ot", -1, 1, 0),
            new Among("isseu", -1, 1, 0),
            new Among("ìsseu", -1, 1, 0),
            new Among("ísseu", -1, 1, 0),
            new Among("triu", -1, 1, 0),
            new Among("íssiu", -1, 1, 0),
            new Among("atiu", -1, 1, 0),
            new Among("ó", -1, 1, 0),
            new Among("ió", 196, 1, 0),
            new Among("ció", 197, 1, 0),
            new Among("ació", 198, 1, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("aba", -1, 1, 0),
            new Among("esca", -1, 1, 0),
            new Among("isca", -1, 1, 0),
            new Among("ïsca", -1, 1, 0),
            new Among("ada", -1, 1, 0),
            new Among("ida", -1, 1, 0),
            new Among("uda", -1, 1, 0),
            new Among("ïda", -1, 1, 0),
            new Among("ia", -1, 1, 0),
            new Among("aria", 8, 1, 0),
            new Among("iria", 8, 1, 0),
            new Among("ara", -1, 1, 0),
            new Among("iera", -1, 1, 0),
            new Among("ira", -1, 1, 0),
            new Among("adora", -1, 1, 0),
            new Among("ïra", -1, 1, 0),
            new Among("ava", -1, 1, 0),
            new Among("ixa", -1, 1, 0),
            new Among("itza", -1, 1, 0),
            new Among("ía", -1, 1, 0),
            new Among("aría", 19, 1, 0),
            new Among("ería", 19, 1, 0),
            new Among("iría", 19, 1, 0),
            new Among("ïa", -1, 1, 0),
            new Among("isc", -1, 1, 0),
            new Among("ïsc", -1, 1, 0),
            new Among("ad", -1, 1, 0),
            new Among("ed", -1, 1, 0),
            new Among("id", -1, 1, 0),
            new Among("ie", -1, 1, 0),
            new Among("re", -1, 1, 0),
            new Among("dre", 30, 1, 0),
            new Among("ase", -1, 1, 0),
            new Among("iese", -1, 1, 0),
            new Among("aste", -1, 1, 0),
            new Among("iste", -1, 1, 0),
            new Among("ii", -1, 1, 0),
            new Among("ini", -1, 1, 0),
            new Among("esqui", -1, 1, 0),
            new Among("eixi", -1, 1, 0),
            new Among("itzi", -1, 1, 0),
            new Among("am", -1, 1, 0),
            new Among("em", -1, 1, 0),
            new Among("arem", 42, 1, 0),
            new Among("irem", 42, 1, 0),
            new Among("àrem", 42, 1, 0),
            new Among("írem", 42, 1, 0),
            new Among("àssem", 42, 1, 0),
            new Among("éssem", 42, 1, 0),
            new Among("iguem", 42, 1, 0),
            new Among("ïguem", 42, 1, 0),
            new Among("avem", 42, 1, 0),
            new Among("àvem", 42, 1, 0),
            new Among("ávem", 42, 1, 0),
            new Among("irìem", 42, 1, 0),
            new Among("íem", 42, 1, 0),
            new Among("aríem", 55, 1, 0),
            new Among("iríem", 55, 1, 0),
            new Among("assim", -1, 1, 0),
            new Among("essim", -1, 1, 0),
            new Among("issim", -1, 1, 0),
            new Among("àssim", -1, 1, 0),
            new Among("èssim", -1, 1, 0),
            new Among("éssim", -1, 1, 0),
            new Among("íssim", -1, 1, 0),
            new Among("ïm", -1, 1, 0),
            new Among("an", -1, 1, 0),
            new Among("aban", 66, 1, 0),
            new Among("arian", 66, 1, 0),
            new Among("aran", 66, 1, 0),
            new Among("ieran", 66, 1, 0),
            new Among("iran", 66, 1, 0),
            new Among("ían", 66, 1, 0),
            new Among("arían", 72, 1, 0),
            new Among("erían", 72, 1, 0),
            new Among("irían", 72, 1, 0),
            new Among("en", -1, 1, 0),
            new Among("ien", 76, 1, 0),
            new Among("arien", 77, 1, 0),
            new Among("irien", 77, 1, 0),
            new Among("aren", 76, 1, 0),
            new Among("eren", 76, 1, 0),
            new Among("iren", 76, 1, 0),
            new Among("àren", 76, 1, 0),
            new Among("ïren", 76, 1, 0),
            new Among("asen", 76, 1, 0),
            new Among("iesen", 76, 1, 0),
            new Among("assen", 76, 1, 0),
            new Among("essen", 76, 1, 0),
            new Among("issen", 76, 1, 0),
            new Among("éssen", 76, 1, 0),
            new Among("ïssen", 76, 1, 0),
            new Among("esquen", 76, 1, 0),
            new Among("isquen", 76, 1, 0),
            new Among("ïsquen", 76, 1, 0),
            new Among("aven", 76, 1, 0),
            new Among("ixen", 76, 1, 0),
            new Among("eixen", 96, 1, 0),
            new Among("ïxen", 76, 1, 0),
            new Among("ïen", 76, 1, 0),
            new Among("in", -1, 1, 0),
            new Among("inin", 100, 1, 0),
            new Among("sin", 100, 1, 0),
            new Among("isin", 102, 1, 0),
            new Among("assin", 102, 1, 0),
            new Among("essin", 102, 1, 0),
            new Among("issin", 102, 1, 0),
            new Among("ïssin", 102, 1, 0),
            new Among("esquin", 100, 1, 0),
            new Among("eixin", 100, 1, 0),
            new Among("aron", -1, 1, 0),
            new Among("ieron", -1, 1, 0),
            new Among("arán", -1, 1, 0),
            new Among("erán", -1, 1, 0),
            new Among("irán", -1, 1, 0),
            new Among("iïn", -1, 1, 0),
            new Among("ado", -1, 1, 0),
            new Among("ido", -1, 1, 0),
            new Among("ando", -1, 2, 0),
            new Among("iendo", -1, 1, 0),
            new Among("io", -1, 1, 0),
            new Among("ixo", -1, 1, 0),
            new Among("eixo", 121, 1, 0),
            new Among("ïxo", -1, 1, 0),
            new Among("itzo", -1, 1, 0),
            new Among("ar", -1, 1, 0),
            new Among("tzar", 125, 1, 0),
            new Among("er", -1, 1, 0),
            new Among("eixer", 127, 1, 0),
            new Among("ir", -1, 1, 0),
            new Among("ador", -1, 1, 0),
            new Among("as", -1, 1, 0),
            new Among("abas", 131, 1, 0),
            new Among("adas", 131, 1, 0),
            new Among("idas", 131, 1, 0),
            new Among("aras", 131, 1, 0),
            new Among("ieras", 131, 1, 0),
            new Among("ías", 131, 1, 0),
            new Among("arías", 137, 1, 0),
            new Among("erías", 137, 1, 0),
            new Among("irías", 137, 1, 0),
            new Among("ids", -1, 1, 0),
            new Among("es", -1, 1, 0),
            new Among("ades", 142, 1, 0),
            new Among("ides", 142, 1, 0),
            new Among("udes", 142, 1, 0),
            new Among("ïdes", 142, 1, 0),
            new Among("atges", 142, 1, 0),
            new Among("ies", 142, 1, 0),
            new Among("aries", 148, 1, 0),
            new Among("iries", 148, 1, 0),
            new Among("ares", 142, 1, 0),
            new Among("ires", 142, 1, 0),
            new Among("adores", 142, 1, 0),
            new Among("ïres", 142, 1, 0),
            new Among("ases", 142, 1, 0),
            new Among("ieses", 142, 1, 0),
            new Among("asses", 142, 1, 0),
            new Among("esses", 142, 1, 0),
            new Among("isses", 142, 1, 0),
            new Among("ïsses", 142, 1, 0),
            new Among("ques", 142, 1, 0),
            new Among("esques", 161, 1, 0),
            new Among("ïsques", 161, 1, 0),
            new Among("aves", 142, 1, 0),
            new Among("ixes", 142, 1, 0),
            new Among("eixes", 165, 1, 0),
            new Among("ïxes", 142, 1, 0),
            new Among("ïes", 142, 1, 0),
            new Among("abais", -1, 1, 0),
            new Among("arais", -1, 1, 0),
            new Among("ierais", -1, 1, 0),
            new Among("íais", -1, 1, 0),
            new Among("aríais", 172, 1, 0),
            new Among("eríais", 172, 1, 0),
            new Among("iríais", 172, 1, 0),
            new Among("aseis", -1, 1, 0),
            new Among("ieseis", -1, 1, 0),
            new Among("asteis", -1, 1, 0),
            new Among("isteis", -1, 1, 0),
            new Among("inis", -1, 1, 0),
            new Among("sis", -1, 1, 0),
            new Among("isis", 181, 1, 0),
            new Among("assis", 181, 1, 0),
            new Among("essis", 181, 1, 0),
            new Among("issis", 181, 1, 0),
            new Among("ïssis", 181, 1, 0),
            new Among("esquis", -1, 1, 0),
            new Among("eixis", -1, 1, 0),
            new Among("itzis", -1, 1, 0),
            new Among("áis", -1, 1, 0),
            new Among("aréis", -1, 1, 0),
            new Among("eréis", -1, 1, 0),
            new Among("iréis", -1, 1, 0),
            new Among("ams", -1, 1, 0),
            new Among("ados", -1, 1, 0),
            new Among("idos", -1, 1, 0),
            new Among("amos", -1, 1, 0),
            new Among("ábamos", 197, 1, 0),
            new Among("áramos", 197, 1, 0),
            new Among("iéramos", 197, 1, 0),
            new Among("íamos", 197, 1, 0),
            new Among("aríamos", 201, 1, 0),
            new Among("eríamos", 201, 1, 0),
            new Among("iríamos", 201, 1, 0),
            new Among("aremos", -1, 1, 0),
            new Among("eremos", -1, 1, 0),
            new Among("iremos", -1, 1, 0),
            new Among("ásemos", -1, 1, 0),
            new Among("iésemos", -1, 1, 0),
            new Among("imos", -1, 1, 0),
            new Among("adors", -1, 1, 0),
            new Among("ass", -1, 1, 0),
            new Among("erass", 212, 1, 0),
            new Among("ess", -1, 1, 0),
            new Among("ats", -1, 1, 0),
            new Among("its", -1, 1, 0),
            new Among("ents", -1, 1, 0),
            new Among("às", -1, 1, 0),
            new Among("aràs", 218, 1, 0),
            new Among("iràs", 218, 1, 0),
            new Among("arás", -1, 1, 0),
            new Among("erás", -1, 1, 0),
            new Among("irás", -1, 1, 0),
            new Among("és", -1, 1, 0),
            new Among("arés", 224, 1, 0),
            new Among("ís", -1, 1, 0),
            new Among("iïs", -1, 1, 0),
            new Among("at", -1, 1, 0),
            new Among("it", -1, 1, 0),
            new Among("ant", -1, 1, 0),
            new Among("ent", -1, 1, 0),
            new Among("int", -1, 1, 0),
            new Among("ut", -1, 1, 0),
            new Among("ït", -1, 1, 0),
            new Among("au", -1, 1, 0),
            new Among("erau", 235, 1, 0),
            new Among("ieu", -1, 1, 0),
            new Among("ineu", -1, 1, 0),
            new Among("areu", -1, 1, 0),
            new Among("ireu", -1, 1, 0),
            new Among("àreu", -1, 1, 0),
            new Among("íreu", -1, 1, 0),
            new Among("asseu", -1, 1, 0),
            new Among("esseu", -1, 1, 0),
            new Among("eresseu", 244, 1, 0),
            new Among("àsseu", -1, 1, 0),
            new Among("ésseu", -1, 1, 0),
            new Among("igueu", -1, 1, 0),
            new Among("ïgueu", -1, 1, 0),
            new Among("àveu", -1, 1, 0),
            new Among("áveu", -1, 1, 0),
            new Among("itzeu", -1, 1, 0),
            new Among("ìeu", -1, 1, 0),
            new Among("irìeu", 253, 1, 0),
            new Among("íeu", -1, 1, 0),
            new Among("aríeu", 255, 1, 0),
            new Among("iríeu", 255, 1, 0),
            new Among("assiu", -1, 1, 0),
            new Among("issiu", -1, 1, 0),
            new Among("àssiu", -1, 1, 0),
            new Among("èssiu", -1, 1, 0),
            new Among("éssiu", -1, 1, 0),
            new Among("íssiu", -1, 1, 0),
            new Among("ïu", -1, 1, 0),
            new Among("ix", -1, 1, 0),
            new Among("eix", 265, 1, 0),
            new Among("ïx", -1, 1, 0),
            new Among("itz", -1, 1, 0),
            new Among("ià", -1, 1, 0),
            new Among("arà", -1, 1, 0),
            new Among("irà", -1, 1, 0),
            new Among("itzà", -1, 1, 0),
            new Among("ará", -1, 1, 0),
            new Among("erá", -1, 1, 0),
            new Among("irá", -1, 1, 0),
            new Among("irè", -1, 1, 0),
            new Among("aré", -1, 1, 0),
            new Among("eré", -1, 1, 0),
            new Among("iré", -1, 1, 0),
            new Among("í", -1, 1, 0),
            new Among("iï", -1, 1, 0),
            new Among("ió", -1, 1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("a", -1, 1, 0),
            new Among("e", -1, 1, 0),
            new Among("i", -1, 1, 0),
            new Among("ïn", -1, 1, 0),
            new Among("o", -1, 1, 0),
            new Among("ir", -1, 1, 0),
            new Among("s", -1, 1, 0),
            new Among("is", 6, 1, 0),
            new Among("os", 6, 1, 0),
            new Among("ïs", 6, 1, 0),
            new Among("it", -1, 1, 0),
            new Among("eu", -1, 1, 0),
            new Among("iu", -1, 1, 0),
            new Among("iqu", -1, 2, 0),
            new Among("itz", -1, 1, 0),
            new Among("à", -1, 1, 0),
            new Among("á", -1, 1, 0),
            new Among("é", -1, 1, 0),
            new Among("ì", -1, 1, 0),
            new Among("í", -1, 1, 0),
            new Among("ï", -1, 1, 0),
            new Among("ó", -1, 1, 0)
        };


        private bool r_mark_regions()
        {
            I_p1 = limit;
            I_p2 = limit;
            {
                int c1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 252, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 252, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p1 = cursor;
                {

                    int ret = out_grouping(g_v, 97, 252, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                {

                    int ret = in_grouping(g_v, 97, 252, true);
                    if (ret < 0)
                    {
                        goto lab0;
                    }

                    cursor += ret;
                }
                I_p2 = cursor;
            lab0: ;
                cursor = c1;
            }
            return true;
        }

        private bool r_cleaning()
        {
            int among_var;
            while (true)
            {
                int c1 = cursor;
                bra = cursor;
                among_var = find_among(a_0, null);
                ket = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("a");
                        break;
                    }
                    case 2: {
                        slice_from("e");
                        break;
                    }
                    case 3: {
                        slice_from("i");
                        break;
                    }
                    case 4: {
                        slice_from("o");
                        break;
                    }
                    case 5: {
                        slice_from("u");
                        break;
                    }
                    case 6: {
                        slice_from(".");
                        break;
                    }
                    case 7: {
                        if (cursor >= limit)
                        {
                            goto lab0;
                        }
                        cursor++;
                        break;
                    }
                }
                continue;
            lab0: ;
                cursor = c1;
                break;
            }
            return true;
        }

        private bool r_R1()
        {
            return I_p1 <= cursor;
        }

        private bool r_R2()
        {
            return I_p2 <= cursor;
        }

        private bool r_attached_pronoun()
        {
            ket = cursor;
            if (find_among_b(a_1, null) == 0)
            {
                return false;
            }
            bra = cursor;
            if (!r_R1())
                return false;
            slice_del();
            return true;
        }

        private bool r_standard_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    break;
                }
                case 3: {
                    if (!r_R2())
                        return false;
                    slice_from("log");
                    break;
                }
                case 4: {
                    if (!r_R2())
                        return false;
                    slice_from("ic");
                    break;
                }
                case 5: {
                    if (!r_R1())
                        return false;
                    slice_from("c");
                    break;
                }
            }
            return true;
        }

        private bool r_verb_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_3, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R2())
                        return false;
                    slice_del();
                    break;
                }
            }
            return true;
        }

        private bool r_residual_suffix()
        {
            int among_var;
            ket = cursor;
            among_var = find_among_b(a_4, null);
            if (among_var == 0)
            {
                return false;
            }
            bra = cursor;
            switch (among_var) {
                case 1: {
                    if (!r_R1())
                        return false;
                    slice_del();
                    break;
                }
                case 2: {
                    if (!r_R1())
                        return false;
                    slice_from("ic");
                    break;
                }
            }
            return true;
        }

        protected override bool stem()
        {
            r_mark_regions();
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                r_attached_pronoun();
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                {
                    int c3 = limit - cursor;
                    if (!r_standard_suffix())
                        goto lab2;
                    goto lab1;
                lab2: ;
                    cursor = limit - c3;
                    if (!r_verb_suffix())
                        goto lab0;
                }
            lab1: ;
            lab0: ;
                cursor = limit - c2;
            }
            {
                int c4 = limit - cursor;
                r_residual_suffix();
                cursor = limit - c4;
            }
            cursor = limit_backward;
            {
                int c5 = cursor;
                r_cleaning();
                cursor = c5;
            }
            return true;
        }

    }
}

