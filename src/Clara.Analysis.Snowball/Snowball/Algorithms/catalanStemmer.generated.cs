// Generated from catalan.sbl by Snowball 3.0.1 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from catalan.sbl by Snowball 3.0.1 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.1")]
    internal partial class CatalanStemmer : Stemmer
    {
        private int I_p2;
        private int I_p1;

        private const string g_v = "aeiouáàéèíïóòúü";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 7),
            new Among("·", 0, 6),
            new Among("à", 0, 1),
            new Among("á", 0, 1),
            new Among("è", 0, 2),
            new Among("é", 0, 2),
            new Among("ì", 0, 3),
            new Among("í", 0, 3),
            new Among("ï", 0, 3),
            new Among("ò", 0, 4),
            new Among("ó", 0, 4),
            new Among("ú", 0, 5),
            new Among("ü", 0, 5)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("la", -1, 1),
            new Among("-la", 0, 1),
            new Among("sela", 0, 1),
            new Among("le", -1, 1),
            new Among("me", -1, 1),
            new Among("-me", 4, 1),
            new Among("se", -1, 1),
            new Among("-te", -1, 1),
            new Among("hi", -1, 1),
            new Among("'hi", 8, 1),
            new Among("li", -1, 1),
            new Among("-li", 10, 1),
            new Among("'l", -1, 1),
            new Among("'m", -1, 1),
            new Among("-m", -1, 1),
            new Among("'n", -1, 1),
            new Among("-n", -1, 1),
            new Among("ho", -1, 1),
            new Among("'ho", 17, 1),
            new Among("lo", -1, 1),
            new Among("selo", 19, 1),
            new Among("'s", -1, 1),
            new Among("las", -1, 1),
            new Among("selas", 22, 1),
            new Among("les", -1, 1),
            new Among("-les", 24, 1),
            new Among("'ls", -1, 1),
            new Among("-ls", -1, 1),
            new Among("'ns", -1, 1),
            new Among("-ns", -1, 1),
            new Among("ens", -1, 1),
            new Among("los", -1, 1),
            new Among("selos", 31, 1),
            new Among("nos", -1, 1),
            new Among("-nos", 33, 1),
            new Among("vos", -1, 1),
            new Among("us", -1, 1),
            new Among("-us", 36, 1),
            new Among("'t", -1, 1)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("ica", -1, 4),
            new Among("lógica", 0, 3),
            new Among("enca", -1, 1),
            new Among("ada", -1, 2),
            new Among("ancia", -1, 1),
            new Among("encia", -1, 1),
            new Among("ència", -1, 1),
            new Among("ícia", -1, 1),
            new Among("logia", -1, 3),
            new Among("inia", -1, 1),
            new Among("íinia", 9, 1),
            new Among("eria", -1, 1),
            new Among("ària", -1, 1),
            new Among("atòria", -1, 1),
            new Among("alla", -1, 1),
            new Among("ella", -1, 1),
            new Among("ívola", -1, 1),
            new Among("ima", -1, 1),
            new Among("íssima", 17, 1),
            new Among("quíssima", 18, 5),
            new Among("ana", -1, 1),
            new Among("ina", -1, 1),
            new Among("era", -1, 1),
            new Among("sfera", 22, 1),
            new Among("ora", -1, 1),
            new Among("dora", 24, 1),
            new Among("adora", 25, 1),
            new Among("adura", -1, 1),
            new Among("esa", -1, 1),
            new Among("osa", -1, 1),
            new Among("assa", -1, 1),
            new Among("essa", -1, 1),
            new Among("issa", -1, 1),
            new Among("eta", -1, 1),
            new Among("ita", -1, 1),
            new Among("ota", -1, 1),
            new Among("ista", -1, 1),
            new Among("ialista", 36, 1),
            new Among("ionista", 36, 1),
            new Among("iva", -1, 1),
            new Among("ativa", 39, 1),
            new Among("nça", -1, 1),
            new Among("logía", -1, 3),
            new Among("ic", -1, 4),
            new Among("ístic", 43, 1),
            new Among("enc", -1, 1),
            new Among("esc", -1, 1),
            new Among("ud", -1, 1),
            new Among("atge", -1, 1),
            new Among("ble", -1, 1),
            new Among("able", 49, 1),
            new Among("ible", 49, 1),
            new Among("isme", -1, 1),
            new Among("ialisme", 52, 1),
            new Among("ionisme", 52, 1),
            new Among("ivisme", 52, 1),
            new Among("aire", -1, 1),
            new Among("icte", -1, 1),
            new Among("iste", -1, 1),
            new Among("ici", -1, 1),
            new Among("íci", -1, 1),
            new Among("logi", -1, 3),
            new Among("ari", -1, 1),
            new Among("tori", -1, 1),
            new Among("al", -1, 1),
            new Among("il", -1, 1),
            new Among("all", -1, 1),
            new Among("ell", -1, 1),
            new Among("ívol", -1, 1),
            new Among("isam", -1, 1),
            new Among("issem", -1, 1),
            new Among("ìssem", -1, 1),
            new Among("íssem", -1, 1),
            new Among("íssim", -1, 1),
            new Among("quíssim", 73, 5),
            new Among("amen", -1, 1),
            new Among("ìssin", -1, 1),
            new Among("ar", -1, 1),
            new Among("ificar", 77, 1),
            new Among("egar", 77, 1),
            new Among("ejar", 77, 1),
            new Among("itar", 77, 1),
            new Among("itzar", 77, 1),
            new Among("fer", -1, 1),
            new Among("or", -1, 1),
            new Among("dor", 84, 1),
            new Among("dur", -1, 1),
            new Among("doras", -1, 1),
            new Among("ics", -1, 4),
            new Among("lógics", 88, 3),
            new Among("uds", -1, 1),
            new Among("nces", -1, 1),
            new Among("ades", -1, 2),
            new Among("ancies", -1, 1),
            new Among("encies", -1, 1),
            new Among("ències", -1, 1),
            new Among("ícies", -1, 1),
            new Among("logies", -1, 3),
            new Among("inies", -1, 1),
            new Among("ínies", -1, 1),
            new Among("eries", -1, 1),
            new Among("àries", -1, 1),
            new Among("atòries", -1, 1),
            new Among("bles", -1, 1),
            new Among("ables", 103, 1),
            new Among("ibles", 103, 1),
            new Among("imes", -1, 1),
            new Among("íssimes", 106, 1),
            new Among("quíssimes", 107, 5),
            new Among("formes", -1, 1),
            new Among("ismes", -1, 1),
            new Among("ialismes", 110, 1),
            new Among("ines", -1, 1),
            new Among("eres", -1, 1),
            new Among("ores", -1, 1),
            new Among("dores", 114, 1),
            new Among("idores", 115, 1),
            new Among("dures", -1, 1),
            new Among("eses", -1, 1),
            new Among("oses", -1, 1),
            new Among("asses", -1, 1),
            new Among("ictes", -1, 1),
            new Among("ites", -1, 1),
            new Among("otes", -1, 1),
            new Among("istes", -1, 1),
            new Among("ialistes", 124, 1),
            new Among("ionistes", 124, 1),
            new Among("iques", -1, 4),
            new Among("lógiques", 127, 3),
            new Among("ives", -1, 1),
            new Among("atives", 129, 1),
            new Among("logíes", -1, 3),
            new Among("allengües", -1, 1),
            new Among("icis", -1, 1),
            new Among("ícis", -1, 1),
            new Among("logis", -1, 3),
            new Among("aris", -1, 1),
            new Among("toris", -1, 1),
            new Among("ls", -1, 1),
            new Among("als", 138, 1),
            new Among("ells", 138, 1),
            new Among("ims", -1, 1),
            new Among("íssims", 141, 1),
            new Among("quíssims", 142, 5),
            new Among("ions", -1, 1),
            new Among("cions", 144, 1),
            new Among("acions", 145, 2),
            new Among("esos", -1, 1),
            new Among("osos", -1, 1),
            new Among("assos", -1, 1),
            new Among("issos", -1, 1),
            new Among("ers", -1, 1),
            new Among("ors", -1, 1),
            new Among("dors", 152, 1),
            new Among("adors", 153, 1),
            new Among("idors", 153, 1),
            new Among("ats", -1, 1),
            new Among("itats", 156, 1),
            new Among("bilitats", 157, 1),
            new Among("ivitats", 157, 1),
            new Among("ativitats", 159, 1),
            new Among("ïtats", 156, 1),
            new Among("ets", -1, 1),
            new Among("ants", -1, 1),
            new Among("ents", -1, 1),
            new Among("ments", 164, 1),
            new Among("aments", 165, 1),
            new Among("ots", -1, 1),
            new Among("uts", -1, 1),
            new Among("ius", -1, 1),
            new Among("trius", 169, 1),
            new Among("atius", 169, 1),
            new Among("ès", -1, 1),
            new Among("és", -1, 1),
            new Among("ís", -1, 1),
            new Among("dís", 174, 1),
            new Among("ós", -1, 1),
            new Among("itat", -1, 1),
            new Among("bilitat", 177, 1),
            new Among("ivitat", 177, 1),
            new Among("ativitat", 179, 1),
            new Among("ïtat", -1, 1),
            new Among("et", -1, 1),
            new Among("ant", -1, 1),
            new Among("ent", -1, 1),
            new Among("ient", 184, 1),
            new Among("ment", 184, 1),
            new Among("ament", 186, 1),
            new Among("isament", 187, 1),
            new Among("ot", -1, 1),
            new Among("isseu", -1, 1),
            new Among("ìsseu", -1, 1),
            new Among("ísseu", -1, 1),
            new Among("triu", -1, 1),
            new Among("íssiu", -1, 1),
            new Among("atiu", -1, 1),
            new Among("ó", -1, 1),
            new Among("ió", 196, 1),
            new Among("ció", 197, 1),
            new Among("ació", 198, 1)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("aba", -1, 1),
            new Among("esca", -1, 1),
            new Among("isca", -1, 1),
            new Among("ïsca", -1, 1),
            new Among("ada", -1, 1),
            new Among("ida", -1, 1),
            new Among("uda", -1, 1),
            new Among("ïda", -1, 1),
            new Among("ia", -1, 1),
            new Among("aria", 8, 1),
            new Among("iria", 8, 1),
            new Among("ara", -1, 1),
            new Among("iera", -1, 1),
            new Among("ira", -1, 1),
            new Among("adora", -1, 1),
            new Among("ïra", -1, 1),
            new Among("ava", -1, 1),
            new Among("ixa", -1, 1),
            new Among("itza", -1, 1),
            new Among("ía", -1, 1),
            new Among("aría", 19, 1),
            new Among("ería", 19, 1),
            new Among("iría", 19, 1),
            new Among("ïa", -1, 1),
            new Among("isc", -1, 1),
            new Among("ïsc", -1, 1),
            new Among("ad", -1, 1),
            new Among("ed", -1, 1),
            new Among("id", -1, 1),
            new Among("ie", -1, 1),
            new Among("re", -1, 1),
            new Among("dre", 30, 1),
            new Among("ase", -1, 1),
            new Among("iese", -1, 1),
            new Among("aste", -1, 1),
            new Among("iste", -1, 1),
            new Among("ii", -1, 1),
            new Among("ini", -1, 1),
            new Among("esqui", -1, 1),
            new Among("eixi", -1, 1),
            new Among("itzi", -1, 1),
            new Among("am", -1, 1),
            new Among("em", -1, 1),
            new Among("arem", 42, 1),
            new Among("irem", 42, 1),
            new Among("àrem", 42, 1),
            new Among("írem", 42, 1),
            new Among("àssem", 42, 1),
            new Among("éssem", 42, 1),
            new Among("iguem", 42, 1),
            new Among("ïguem", 42, 1),
            new Among("avem", 42, 1),
            new Among("àvem", 42, 1),
            new Among("ávem", 42, 1),
            new Among("irìem", 42, 1),
            new Among("íem", 42, 1),
            new Among("aríem", 55, 1),
            new Among("iríem", 55, 1),
            new Among("assim", -1, 1),
            new Among("essim", -1, 1),
            new Among("issim", -1, 1),
            new Among("àssim", -1, 1),
            new Among("èssim", -1, 1),
            new Among("éssim", -1, 1),
            new Among("íssim", -1, 1),
            new Among("ïm", -1, 1),
            new Among("an", -1, 1),
            new Among("aban", 66, 1),
            new Among("arian", 66, 1),
            new Among("aran", 66, 1),
            new Among("ieran", 66, 1),
            new Among("iran", 66, 1),
            new Among("ían", 66, 1),
            new Among("arían", 72, 1),
            new Among("erían", 72, 1),
            new Among("irían", 72, 1),
            new Among("en", -1, 1),
            new Among("ien", 76, 1),
            new Among("arien", 77, 1),
            new Among("irien", 77, 1),
            new Among("aren", 76, 1),
            new Among("eren", 76, 1),
            new Among("iren", 76, 1),
            new Among("àren", 76, 1),
            new Among("ïren", 76, 1),
            new Among("asen", 76, 1),
            new Among("iesen", 76, 1),
            new Among("assen", 76, 1),
            new Among("essen", 76, 1),
            new Among("issen", 76, 1),
            new Among("éssen", 76, 1),
            new Among("ïssen", 76, 1),
            new Among("esquen", 76, 1),
            new Among("isquen", 76, 1),
            new Among("ïsquen", 76, 1),
            new Among("aven", 76, 1),
            new Among("ixen", 76, 1),
            new Among("eixen", 96, 1),
            new Among("ïxen", 76, 1),
            new Among("ïen", 76, 1),
            new Among("in", -1, 1),
            new Among("inin", 100, 1),
            new Among("sin", 100, 1),
            new Among("isin", 102, 1),
            new Among("assin", 102, 1),
            new Among("essin", 102, 1),
            new Among("issin", 102, 1),
            new Among("ïssin", 102, 1),
            new Among("esquin", 100, 1),
            new Among("eixin", 100, 1),
            new Among("aron", -1, 1),
            new Among("ieron", -1, 1),
            new Among("arán", -1, 1),
            new Among("erán", -1, 1),
            new Among("irán", -1, 1),
            new Among("iïn", -1, 1),
            new Among("ado", -1, 1),
            new Among("ido", -1, 1),
            new Among("ando", -1, 2),
            new Among("iendo", -1, 1),
            new Among("io", -1, 1),
            new Among("ixo", -1, 1),
            new Among("eixo", 121, 1),
            new Among("ïxo", -1, 1),
            new Among("itzo", -1, 1),
            new Among("ar", -1, 1),
            new Among("tzar", 125, 1),
            new Among("er", -1, 1),
            new Among("eixer", 127, 1),
            new Among("ir", -1, 1),
            new Among("ador", -1, 1),
            new Among("as", -1, 1),
            new Among("abas", 131, 1),
            new Among("adas", 131, 1),
            new Among("idas", 131, 1),
            new Among("aras", 131, 1),
            new Among("ieras", 131, 1),
            new Among("ías", 131, 1),
            new Among("arías", 137, 1),
            new Among("erías", 137, 1),
            new Among("irías", 137, 1),
            new Among("ids", -1, 1),
            new Among("es", -1, 1),
            new Among("ades", 142, 1),
            new Among("ides", 142, 1),
            new Among("udes", 142, 1),
            new Among("ïdes", 142, 1),
            new Among("atges", 142, 1),
            new Among("ies", 142, 1),
            new Among("aries", 148, 1),
            new Among("iries", 148, 1),
            new Among("ares", 142, 1),
            new Among("ires", 142, 1),
            new Among("adores", 142, 1),
            new Among("ïres", 142, 1),
            new Among("ases", 142, 1),
            new Among("ieses", 142, 1),
            new Among("asses", 142, 1),
            new Among("esses", 142, 1),
            new Among("isses", 142, 1),
            new Among("ïsses", 142, 1),
            new Among("ques", 142, 1),
            new Among("esques", 161, 1),
            new Among("ïsques", 161, 1),
            new Among("aves", 142, 1),
            new Among("ixes", 142, 1),
            new Among("eixes", 165, 1),
            new Among("ïxes", 142, 1),
            new Among("ïes", 142, 1),
            new Among("abais", -1, 1),
            new Among("arais", -1, 1),
            new Among("ierais", -1, 1),
            new Among("íais", -1, 1),
            new Among("aríais", 172, 1),
            new Among("eríais", 172, 1),
            new Among("iríais", 172, 1),
            new Among("aseis", -1, 1),
            new Among("ieseis", -1, 1),
            new Among("asteis", -1, 1),
            new Among("isteis", -1, 1),
            new Among("inis", -1, 1),
            new Among("sis", -1, 1),
            new Among("isis", 181, 1),
            new Among("assis", 181, 1),
            new Among("essis", 181, 1),
            new Among("issis", 181, 1),
            new Among("ïssis", 181, 1),
            new Among("esquis", -1, 1),
            new Among("eixis", -1, 1),
            new Among("itzis", -1, 1),
            new Among("áis", -1, 1),
            new Among("aréis", -1, 1),
            new Among("eréis", -1, 1),
            new Among("iréis", -1, 1),
            new Among("ams", -1, 1),
            new Among("ados", -1, 1),
            new Among("idos", -1, 1),
            new Among("amos", -1, 1),
            new Among("ábamos", 197, 1),
            new Among("áramos", 197, 1),
            new Among("iéramos", 197, 1),
            new Among("íamos", 197, 1),
            new Among("aríamos", 201, 1),
            new Among("eríamos", 201, 1),
            new Among("iríamos", 201, 1),
            new Among("aremos", -1, 1),
            new Among("eremos", -1, 1),
            new Among("iremos", -1, 1),
            new Among("ásemos", -1, 1),
            new Among("iésemos", -1, 1),
            new Among("imos", -1, 1),
            new Among("adors", -1, 1),
            new Among("ass", -1, 1),
            new Among("erass", 212, 1),
            new Among("ess", -1, 1),
            new Among("ats", -1, 1),
            new Among("its", -1, 1),
            new Among("ents", -1, 1),
            new Among("às", -1, 1),
            new Among("aràs", 218, 1),
            new Among("iràs", 218, 1),
            new Among("arás", -1, 1),
            new Among("erás", -1, 1),
            new Among("irás", -1, 1),
            new Among("és", -1, 1),
            new Among("arés", 224, 1),
            new Among("ís", -1, 1),
            new Among("iïs", -1, 1),
            new Among("at", -1, 1),
            new Among("it", -1, 1),
            new Among("ant", -1, 1),
            new Among("ent", -1, 1),
            new Among("int", -1, 1),
            new Among("ut", -1, 1),
            new Among("ït", -1, 1),
            new Among("au", -1, 1),
            new Among("erau", 235, 1),
            new Among("ieu", -1, 1),
            new Among("ineu", -1, 1),
            new Among("areu", -1, 1),
            new Among("ireu", -1, 1),
            new Among("àreu", -1, 1),
            new Among("íreu", -1, 1),
            new Among("asseu", -1, 1),
            new Among("esseu", -1, 1),
            new Among("eresseu", 244, 1),
            new Among("àsseu", -1, 1),
            new Among("ésseu", -1, 1),
            new Among("igueu", -1, 1),
            new Among("ïgueu", -1, 1),
            new Among("àveu", -1, 1),
            new Among("áveu", -1, 1),
            new Among("itzeu", -1, 1),
            new Among("ìeu", -1, 1),
            new Among("irìeu", 253, 1),
            new Among("íeu", -1, 1),
            new Among("aríeu", 255, 1),
            new Among("iríeu", 255, 1),
            new Among("assiu", -1, 1),
            new Among("issiu", -1, 1),
            new Among("àssiu", -1, 1),
            new Among("èssiu", -1, 1),
            new Among("éssiu", -1, 1),
            new Among("íssiu", -1, 1),
            new Among("ïu", -1, 1),
            new Among("ix", -1, 1),
            new Among("eix", 265, 1),
            new Among("ïx", -1, 1),
            new Among("itz", -1, 1),
            new Among("ià", -1, 1),
            new Among("arà", -1, 1),
            new Among("irà", -1, 1),
            new Among("itzà", -1, 1),
            new Among("ará", -1, 1),
            new Among("erá", -1, 1),
            new Among("irá", -1, 1),
            new Among("irè", -1, 1),
            new Among("aré", -1, 1),
            new Among("eré", -1, 1),
            new Among("iré", -1, 1),
            new Among("í", -1, 1),
            new Among("iï", -1, 1),
            new Among("ió", -1, 1)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("a", -1, 1),
            new Among("e", -1, 1),
            new Among("i", -1, 1),
            new Among("ïn", -1, 1),
            new Among("o", -1, 1),
            new Among("ir", -1, 1),
            new Among("s", -1, 1),
            new Among("is", 6, 1),
            new Among("os", 6, 1),
            new Among("ïs", 6, 1),
            new Among("it", -1, 1),
            new Among("eu", -1, 1),
            new Among("iu", -1, 1),
            new Among("iqu", -1, 2),
            new Among("itz", -1, 1),
            new Among("à", -1, 1),
            new Among("á", -1, 1),
            new Among("é", -1, 1),
            new Among("ì", -1, 1),
            new Among("í", -1, 1),
            new Among("ï", -1, 1),
            new Among("ó", -1, 1)
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
                among_var = find_among(a_0);
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
            if (find_among_b(a_1) == 0)
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
            among_var = find_among_b(a_2);
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
            among_var = find_among_b(a_3);
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
            among_var = find_among_b(a_4);
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

