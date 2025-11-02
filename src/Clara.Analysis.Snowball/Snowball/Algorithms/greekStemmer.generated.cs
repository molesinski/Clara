// Generated from greek.sbl by Snowball 3.0.0 - https://snowballstem.org/

#pragma warning disable 0164
#pragma warning disable 0162

namespace Snowball
{
    using System;
    using System.Text;

    ///<summary>
    ///  This class implements the stemming algorithm defined by a snowball script.
    ///  Generated from greek.sbl by Snowball 3.0.0 - https://snowballstem.org/
    ///</summary>
    ///
    [System.CodeDom.Compiler.GeneratedCode("Snowball", "3.0.0")]
    internal partial class GreekStemmer : Stemmer
    {
        private bool B_test1;

        private const string g_v = "αεηιουω";
        private const string g_v2 = "αεηιοω";

        private static readonly Among[] a_0 = new[]
        {
            new Among("", -1, 25, 0),
            new Among("Ά", 0, 1, 0),
            new Among("Έ", 0, 5, 0),
            new Among("Ή", 0, 7, 0),
            new Among("Ί", 0, 9, 0),
            new Among("Ό", 0, 15, 0),
            new Among("Ύ", 0, 20, 0),
            new Among("Ώ", 0, 24, 0),
            new Among("ΐ", 0, 7, 0),
            new Among("Α", 0, 1, 0),
            new Among("Β", 0, 2, 0),
            new Among("Γ", 0, 3, 0),
            new Among("Δ", 0, 4, 0),
            new Among("Ε", 0, 5, 0),
            new Among("Ζ", 0, 6, 0),
            new Among("Η", 0, 7, 0),
            new Among("Θ", 0, 8, 0),
            new Among("Ι", 0, 9, 0),
            new Among("Κ", 0, 10, 0),
            new Among("Λ", 0, 11, 0),
            new Among("Μ", 0, 12, 0),
            new Among("Ν", 0, 13, 0),
            new Among("Ξ", 0, 14, 0),
            new Among("Ο", 0, 15, 0),
            new Among("Π", 0, 16, 0),
            new Among("Ρ", 0, 17, 0),
            new Among("Σ", 0, 18, 0),
            new Among("Τ", 0, 19, 0),
            new Among("Υ", 0, 20, 0),
            new Among("Φ", 0, 21, 0),
            new Among("Χ", 0, 22, 0),
            new Among("Ψ", 0, 23, 0),
            new Among("Ω", 0, 24, 0),
            new Among("Ϊ", 0, 9, 0),
            new Among("Ϋ", 0, 20, 0),
            new Among("ά", 0, 1, 0),
            new Among("έ", 0, 5, 0),
            new Among("ή", 0, 7, 0),
            new Among("ί", 0, 9, 0),
            new Among("ΰ", 0, 20, 0),
            new Among("ς", 0, 18, 0),
            new Among("ϊ", 0, 7, 0),
            new Among("ϋ", 0, 20, 0),
            new Among("ό", 0, 15, 0),
            new Among("ύ", 0, 20, 0),
            new Among("ώ", 0, 24, 0)
        };

        private static readonly Among[] a_1 = new[]
        {
            new Among("σκαγια", -1, 2, 0),
            new Among("φαγια", -1, 1, 0),
            new Among("ολογια", -1, 3, 0),
            new Among("σογια", -1, 4, 0),
            new Among("τατογια", -1, 5, 0),
            new Among("κρεατα", -1, 6, 0),
            new Among("περατα", -1, 7, 0),
            new Among("τερατα", -1, 8, 0),
            new Among("γεγονοτα", -1, 11, 0),
            new Among("καθεστωτα", -1, 10, 0),
            new Among("φωτα", -1, 9, 0),
            new Among("περατη", -1, 7, 0),
            new Among("σκαγιων", -1, 2, 0),
            new Among("φαγιων", -1, 1, 0),
            new Among("ολογιων", -1, 3, 0),
            new Among("σογιων", -1, 4, 0),
            new Among("τατογιων", -1, 5, 0),
            new Among("κρεατων", -1, 6, 0),
            new Among("περατων", -1, 7, 0),
            new Among("τερατων", -1, 8, 0),
            new Among("γεγονοτων", -1, 11, 0),
            new Among("καθεστωτων", -1, 10, 0),
            new Among("φωτων", -1, 9, 0),
            new Among("κρεασ", -1, 6, 0),
            new Among("περασ", -1, 7, 0),
            new Among("τερασ", -1, 8, 0),
            new Among("γεγονοσ", -1, 11, 0),
            new Among("κρεατοσ", -1, 6, 0),
            new Among("περατοσ", -1, 7, 0),
            new Among("τερατοσ", -1, 8, 0),
            new Among("γεγονοτοσ", -1, 11, 0),
            new Among("καθεστωτοσ", -1, 10, 0),
            new Among("φωτοσ", -1, 9, 0),
            new Among("καθεστωσ", -1, 10, 0),
            new Among("φωσ", -1, 9, 0),
            new Among("σκαγιου", -1, 2, 0),
            new Among("φαγιου", -1, 1, 0),
            new Among("ολογιου", -1, 3, 0),
            new Among("σογιου", -1, 4, 0),
            new Among("τατογιου", -1, 5, 0)
        };

        private static readonly Among[] a_2 = new[]
        {
            new Among("πα", -1, 1, 0),
            new Among("ξαναπα", 0, 1, 0),
            new Among("επα", 0, 1, 0),
            new Among("περιπα", 0, 1, 0),
            new Among("αναμπα", 0, 1, 0),
            new Among("εμπα", 0, 1, 0),
            new Among("β", -1, 2, 0),
            new Among("δανε", -1, 1, 0),
            new Among("βαθυρι", -1, 2, 0),
            new Among("βαρκ", -1, 2, 0),
            new Among("μαρκ", -1, 2, 0),
            new Among("λ", -1, 2, 0),
            new Among("μ", -1, 2, 0),
            new Among("κορν", -1, 2, 0),
            new Among("αθρο", -1, 1, 0),
            new Among("συναθρο", 14, 1, 0),
            new Among("π", -1, 2, 0),
            new Among("ιμπ", 16, 2, 0),
            new Among("ρ", -1, 2, 0),
            new Among("μαρ", 18, 2, 0),
            new Among("αμπαρ", 18, 2, 0),
            new Among("γκρ", 18, 2, 0),
            new Among("βολβορ", 18, 2, 0),
            new Among("γλυκορ", 18, 2, 0),
            new Among("πιπερορ", 18, 2, 0),
            new Among("πρ", 18, 2, 0),
            new Among("μπρ", 25, 2, 0),
            new Among("αρρ", 18, 2, 0),
            new Among("γλυκυρ", 18, 2, 0),
            new Among("πολυρ", 18, 2, 0),
            new Among("λου", -1, 2, 0)
        };

        private static readonly Among[] a_3 = new[]
        {
            new Among("ιζα", -1, 1, 0),
            new Among("ιζε", -1, 1, 0),
            new Among("ιζαμε", -1, 1, 0),
            new Among("ιζουμε", -1, 1, 0),
            new Among("ιζανε", -1, 1, 0),
            new Among("ιζουνε", -1, 1, 0),
            new Among("ιζατε", -1, 1, 0),
            new Among("ιζετε", -1, 1, 0),
            new Among("ιζει", -1, 1, 0),
            new Among("ιζαν", -1, 1, 0),
            new Among("ιζουν", -1, 1, 0),
            new Among("ιζεσ", -1, 1, 0),
            new Among("ιζεισ", -1, 1, 0),
            new Among("ιζω", -1, 1, 0)
        };

        private static readonly Among[] a_4 = new[]
        {
            new Among("βι", -1, 1, 0),
            new Among("λι", -1, 1, 0),
            new Among("αλ", -1, 1, 0),
            new Among("εν", -1, 1, 0),
            new Among("σ", -1, 1, 0),
            new Among("χ", -1, 1, 0),
            new Among("υψ", -1, 1, 0),
            new Among("ζω", -1, 1, 0)
        };

        private static readonly Among[] a_5 = new[]
        {
            new Among("ωθηκα", -1, 1, 0),
            new Among("ωθηκε", -1, 1, 0),
            new Among("ωθηκαμε", -1, 1, 0),
            new Among("ωθηκανε", -1, 1, 0),
            new Among("ωθηκατε", -1, 1, 0),
            new Among("ωθηκαν", -1, 1, 0),
            new Among("ωθηκεσ", -1, 1, 0)
        };

        private static readonly Among[] a_6 = new[]
        {
            new Among("ξαναπα", -1, 1, 0),
            new Among("επα", -1, 1, 0),
            new Among("περιπα", -1, 1, 0),
            new Among("αναμπα", -1, 1, 0),
            new Among("εμπα", -1, 1, 0),
            new Among("χαρτοπα", -1, 1, 0),
            new Among("εξαρχα", -1, 1, 0),
            new Among("γε", -1, 2, 0),
            new Among("γκε", -1, 2, 0),
            new Among("κλε", -1, 1, 0),
            new Among("εκλε", 9, 1, 0),
            new Among("απεκλε", 10, 1, 0),
            new Among("αποκλε", 9, 1, 0),
            new Among("εσωκλε", 9, 1, 0),
            new Among("δανε", -1, 1, 0),
            new Among("πε", -1, 1, 0),
            new Among("επε", 15, 1, 0),
            new Among("μετεπε", 16, 1, 0),
            new Among("εσε", -1, 1, 0),
            new Among("γκ", -1, 2, 0),
            new Among("μ", -1, 2, 0),
            new Among("πουκαμ", 20, 2, 0),
            new Among("κομ", 20, 2, 0),
            new Among("αν", -1, 2, 0),
            new Among("ολο", -1, 2, 0),
            new Among("αθρο", -1, 1, 0),
            new Among("συναθρο", 25, 1, 0),
            new Among("π", -1, 2, 0),
            new Among("λαρ", -1, 2, 0),
            new Among("δημοκρατ", -1, 2, 0),
            new Among("αφ", -1, 2, 0),
            new Among("γιγαντοαφ", 30, 2, 0)
        };

        private static readonly Among[] a_7 = new[]
        {
            new Among("ισα", -1, 1, 0),
            new Among("ισαμε", -1, 1, 0),
            new Among("ισανε", -1, 1, 0),
            new Among("ισε", -1, 1, 0),
            new Among("ισατε", -1, 1, 0),
            new Among("ισαν", -1, 1, 0),
            new Among("ισεσ", -1, 1, 0)
        };

        private static readonly Among[] a_8 = new[]
        {
            new Among("ξαναπα", -1, 1, 0),
            new Among("επα", -1, 1, 0),
            new Among("περιπα", -1, 1, 0),
            new Among("αναμπα", -1, 1, 0),
            new Among("εμπα", -1, 1, 0),
            new Among("χαρτοπα", -1, 1, 0),
            new Among("εξαρχα", -1, 1, 0),
            new Among("κλε", -1, 1, 0),
            new Among("εκλε", 7, 1, 0),
            new Among("απεκλε", 8, 1, 0),
            new Among("αποκλε", 7, 1, 0),
            new Among("εσωκλε", 7, 1, 0),
            new Among("δανε", -1, 1, 0),
            new Among("πε", -1, 1, 0),
            new Among("επε", 13, 1, 0),
            new Among("μετεπε", 14, 1, 0),
            new Among("εσε", -1, 1, 0),
            new Among("αθρο", -1, 1, 0),
            new Among("συναθρο", 17, 1, 0)
        };

        private static readonly Among[] a_9 = new[]
        {
            new Among("ισουμε", -1, 1, 0),
            new Among("ισουνε", -1, 1, 0),
            new Among("ισετε", -1, 1, 0),
            new Among("ισει", -1, 1, 0),
            new Among("ισουν", -1, 1, 0),
            new Among("ισεισ", -1, 1, 0),
            new Among("ισω", -1, 1, 0)
        };

        private static readonly Among[] a_10 = new[]
        {
            new Among("ατα", -1, 2, 0),
            new Among("φα", -1, 2, 0),
            new Among("ηφα", 1, 2, 0),
            new Among("μεγ", -1, 2, 0),
            new Among("λυγ", -1, 2, 0),
            new Among("ηδ", -1, 2, 0),
            new Among("κλε", -1, 1, 0),
            new Among("εσωκλε", 6, 1, 0),
            new Among("πλε", -1, 1, 0),
            new Among("δανε", -1, 1, 0),
            new Among("σε", -1, 1, 0),
            new Among("ασε", 10, 1, 0),
            new Among("καθ", -1, 2, 0),
            new Among("εχθ", -1, 2, 0),
            new Among("κακ", -1, 2, 0),
            new Among("μακ", -1, 2, 0),
            new Among("σκ", -1, 2, 0),
            new Among("φιλ", -1, 2, 0),
            new Among("κυλ", -1, 2, 0),
            new Among("μ", -1, 2, 0),
            new Among("γεμ", 19, 2, 0),
            new Among("αχν", -1, 2, 0),
            new Among("συναθρο", -1, 1, 0),
            new Among("π", -1, 2, 0),
            new Among("απ", 23, 2, 0),
            new Among("εμπ", 23, 2, 0),
            new Among("ευπ", 23, 2, 0),
            new Among("αρ", -1, 2, 0),
            new Among("αορ", -1, 2, 0),
            new Among("γυρ", -1, 2, 0),
            new Among("χρ", -1, 2, 0),
            new Among("χωρ", -1, 2, 0),
            new Among("κτ", -1, 2, 0),
            new Among("ακτ", 32, 2, 0),
            new Among("χτ", -1, 2, 0),
            new Among("αχτ", 34, 2, 0),
            new Among("ταχ", -1, 2, 0),
            new Among("σχ", -1, 2, 0),
            new Among("ασχ", 37, 2, 0),
            new Among("υψ", -1, 2, 0)
        };

        private static readonly Among[] a_11 = new[]
        {
            new Among("ιστα", -1, 1, 0),
            new Among("ιστε", -1, 1, 0),
            new Among("ιστη", -1, 1, 0),
            new Among("ιστοι", -1, 1, 0),
            new Among("ιστων", -1, 1, 0),
            new Among("ιστο", -1, 1, 0),
            new Among("ιστεσ", -1, 1, 0),
            new Among("ιστησ", -1, 1, 0),
            new Among("ιστοσ", -1, 1, 0),
            new Among("ιστουσ", -1, 1, 0),
            new Among("ιστου", -1, 1, 0)
        };

        private static readonly Among[] a_12 = new[]
        {
            new Among("εγκλε", -1, 1, 0),
            new Among("αποκλε", -1, 1, 0),
            new Among("δανε", -1, 2, 0),
            new Among("αντιδανε", 2, 2, 0),
            new Among("σε", -1, 1, 0),
            new Among("μετασε", 4, 1, 0),
            new Among("μικροσε", 4, 1, 0)
        };

        private static readonly Among[] a_13 = new[]
        {
            new Among("ατομικ", -1, 2, 0),
            new Among("εθνικ", -1, 4, 0),
            new Among("τοπικ", -1, 7, 0),
            new Among("εκλεκτικ", -1, 5, 0),
            new Among("σκεπτικ", -1, 6, 0),
            new Among("γνωστικ", -1, 3, 0),
            new Among("αγνωστικ", 5, 1, 0),
            new Among("αλεξανδριν", -1, 8, 0),
            new Among("θεατριν", -1, 10, 0),
            new Among("βυζαντιν", -1, 9, 0)
        };

        private static readonly Among[] a_14 = new[]
        {
            new Among("ισμοι", -1, 1, 0),
            new Among("ισμων", -1, 1, 0),
            new Among("ισμο", -1, 1, 0),
            new Among("ισμοσ", -1, 1, 0),
            new Among("ισμουσ", -1, 1, 0),
            new Among("ισμου", -1, 1, 0)
        };

        private static readonly Among[] a_15 = new[]
        {
            new Among("σ", -1, 1, 0),
            new Among("χ", -1, 1, 0)
        };

        private static readonly Among[] a_16 = new[]
        {
            new Among("ουδακια", -1, 1, 0),
            new Among("αρακια", -1, 1, 0),
            new Among("ουδακι", -1, 1, 0),
            new Among("αρακι", -1, 1, 0)
        };

        private static readonly Among[] a_17 = new[]
        {
            new Among("β", -1, 2, 0),
            new Among("βαμβ", 0, 1, 0),
            new Among("σλοβ", 0, 1, 0),
            new Among("τσεχοσλοβ", 2, 1, 0),
            new Among("καρδ", -1, 2, 0),
            new Among("ζ", -1, 2, 0),
            new Among("τζ", 5, 1, 0),
            new Among("κ", -1, 1, 0),
            new Among("καπακ", 7, 1, 0),
            new Among("σοκ", 7, 1, 0),
            new Among("σκ", 7, 1, 0),
            new Among("βαλ", -1, 2, 0),
            new Among("μαλ", -1, 1, 0),
            new Among("γλ", -1, 2, 0),
            new Among("τριπολ", -1, 2, 0),
            new Among("πλ", -1, 1, 0),
            new Among("λουλ", -1, 1, 0),
            new Among("φυλ", -1, 1, 0),
            new Among("καιμ", -1, 1, 0),
            new Among("κλιμ", -1, 1, 0),
            new Among("φαρμ", -1, 1, 0),
            new Among("γιαν", -1, 2, 0),
            new Among("σπαν", -1, 1, 0),
            new Among("ηγουμεν", -1, 2, 0),
            new Among("κον", -1, 1, 0),
            new Among("μακρυν", -1, 2, 0),
            new Among("π", -1, 2, 0),
            new Among("κατραπ", 26, 1, 0),
            new Among("ρ", -1, 1, 0),
            new Among("βρ", 28, 1, 0),
            new Among("λαβρ", 29, 1, 0),
            new Among("αμβρ", 29, 1, 0),
            new Among("μερ", 28, 1, 0),
            new Among("πατερ", 28, 2, 0),
            new Among("ανθρ", 28, 1, 0),
            new Among("κορ", 28, 1, 0),
            new Among("σ", -1, 1, 0),
            new Among("ναγκασ", 36, 1, 0),
            new Among("τοσ", 36, 2, 0),
            new Among("μουστ", -1, 1, 0),
            new Among("ρυ", -1, 1, 0),
            new Among("φ", -1, 1, 0),
            new Among("σφ", 41, 1, 0),
            new Among("αλισφ", 42, 1, 0),
            new Among("νυφ", 41, 2, 0),
            new Among("χ", -1, 1, 0)
        };

        private static readonly Among[] a_18 = new[]
        {
            new Among("ακια", -1, 1, 0),
            new Among("αρακια", 0, 1, 0),
            new Among("ιτσα", -1, 1, 0),
            new Among("ακι", -1, 1, 0),
            new Among("αρακι", 3, 1, 0),
            new Among("ιτσων", -1, 1, 0),
            new Among("ιτσασ", -1, 1, 0),
            new Among("ιτσεσ", -1, 1, 0)
        };

        private static readonly Among[] a_19 = new[]
        {
            new Among("ψαλ", -1, 1, 0),
            new Among("αιφν", -1, 1, 0),
            new Among("ολο", -1, 1, 0),
            new Among("ιρ", -1, 1, 0)
        };

        private static readonly Among[] a_20 = new[]
        {
            new Among("ε", -1, 1, 0),
            new Among("παιχν", -1, 1, 0)
        };

        private static readonly Among[] a_21 = new[]
        {
            new Among("ιδια", -1, 1, 0),
            new Among("ιδιων", -1, 1, 0),
            new Among("ιδιο", -1, 1, 0)
        };

        private static readonly Among[] a_22 = new[]
        {
            new Among("ιβ", -1, 1, 0),
            new Among("δ", -1, 1, 0),
            new Among("φραγκ", -1, 1, 0),
            new Among("λυκ", -1, 1, 0),
            new Among("οβελ", -1, 1, 0),
            new Among("μην", -1, 1, 0),
            new Among("ρ", -1, 1, 0)
        };

        private static readonly Among[] a_23 = new[]
        {
            new Among("ισκε", -1, 1, 0),
            new Among("ισκο", -1, 1, 0),
            new Among("ισκοσ", -1, 1, 0),
            new Among("ισκου", -1, 1, 0)
        };

        private static readonly Among[] a_24 = new[]
        {
            new Among("αδων", -1, 1, 0),
            new Among("αδεσ", -1, 1, 0)
        };

        private static readonly Among[] a_25 = new[]
        {
            new Among("γιαγι", -1, -1, 0),
            new Among("θει", -1, -1, 0),
            new Among("οκ", -1, -1, 0),
            new Among("μαμ", -1, -1, 0),
            new Among("μαν", -1, -1, 0),
            new Among("μπαμπ", -1, -1, 0),
            new Among("πεθερ", -1, -1, 0),
            new Among("πατερ", -1, -1, 0),
            new Among("κυρ", -1, -1, 0),
            new Among("νταντ", -1, -1, 0)
        };

        private static readonly Among[] a_26 = new[]
        {
            new Among("εδων", -1, 1, 0),
            new Among("εδεσ", -1, 1, 0)
        };

        private static readonly Among[] a_27 = new[]
        {
            new Among("μιλ", -1, 1, 0),
            new Among("δαπ", -1, 1, 0),
            new Among("γηπ", -1, 1, 0),
            new Among("ιπ", -1, 1, 0),
            new Among("εμπ", -1, 1, 0),
            new Among("οπ", -1, 1, 0),
            new Among("κρασπ", -1, 1, 0),
            new Among("υπ", -1, 1, 0)
        };

        private static readonly Among[] a_28 = new[]
        {
            new Among("ουδων", -1, 1, 0),
            new Among("ουδεσ", -1, 1, 0)
        };

        private static readonly Among[] a_29 = new[]
        {
            new Among("τραγ", -1, 1, 0),
            new Among("φε", -1, 1, 0),
            new Among("καλιακ", -1, 1, 0),
            new Among("αρκ", -1, 1, 0),
            new Among("σκ", -1, 1, 0),
            new Among("πεταλ", -1, 1, 0),
            new Among("βελ", -1, 1, 0),
            new Among("λουλ", -1, 1, 0),
            new Among("φλ", -1, 1, 0),
            new Among("χν", -1, 1, 0),
            new Among("πλεξ", -1, 1, 0),
            new Among("σπ", -1, 1, 0),
            new Among("φρ", -1, 1, 0),
            new Among("σ", -1, 1, 0),
            new Among("λιχ", -1, 1, 0)
        };

        private static readonly Among[] a_30 = new[]
        {
            new Among("εων", -1, 1, 0),
            new Among("εωσ", -1, 1, 0)
        };

        private static readonly Among[] a_31 = new[]
        {
            new Among("δ", -1, 1, 0),
            new Among("ιδ", 0, 1, 0),
            new Among("θ", -1, 1, 0),
            new Among("γαλ", -1, 1, 0),
            new Among("ελ", -1, 1, 0),
            new Among("ν", -1, 1, 0),
            new Among("π", -1, 1, 0),
            new Among("παρ", -1, 1, 0)
        };

        private static readonly Among[] a_32 = new[]
        {
            new Among("ια", -1, 1, 0),
            new Among("ιων", -1, 1, 0),
            new Among("ιου", -1, 1, 0)
        };

        private static readonly Among[] a_33 = new[]
        {
            new Among("ικα", -1, 1, 0),
            new Among("ικων", -1, 1, 0),
            new Among("ικο", -1, 1, 0),
            new Among("ικου", -1, 1, 0)
        };

        private static readonly Among[] a_34 = new[]
        {
            new Among("αδ", -1, 1, 0),
            new Among("συναδ", 0, 1, 0),
            new Among("καταδ", 0, 1, 0),
            new Among("αντιδ", -1, 1, 0),
            new Among("ενδ", -1, 1, 0),
            new Among("φυλοδ", -1, 1, 0),
            new Among("υποδ", -1, 1, 0),
            new Among("πρωτοδ", -1, 1, 0),
            new Among("εξωδ", -1, 1, 0),
            new Among("ηθ", -1, 1, 0),
            new Among("ανηθ", 9, 1, 0),
            new Among("ξικ", -1, 1, 0),
            new Among("αλ", -1, 1, 0),
            new Among("αμμοχαλ", 12, 1, 0),
            new Among("συνομηλ", -1, 1, 0),
            new Among("μπολ", -1, 1, 0),
            new Among("μουλ", -1, 1, 0),
            new Among("τσαμ", -1, 1, 0),
            new Among("βρωμ", -1, 1, 0),
            new Among("αμαν", -1, 1, 0),
            new Among("μπαν", -1, 1, 0),
            new Among("καλλιν", -1, 1, 0),
            new Among("ποστελν", -1, 1, 0),
            new Among("φιλον", -1, 1, 0),
            new Among("καλπ", -1, 1, 0),
            new Among("γερ", -1, 1, 0),
            new Among("χασ", -1, 1, 0),
            new Among("μποσ", -1, 1, 0),
            new Among("πλιατσ", -1, 1, 0),
            new Among("πετσ", -1, 1, 0),
            new Among("πιτσ", -1, 1, 0),
            new Among("φυσ", -1, 1, 0),
            new Among("μπαγιατ", -1, 1, 0),
            new Among("νιτ", -1, 1, 0),
            new Among("πικαντ", -1, 1, 0),
            new Among("σερτ", -1, 1, 0)
        };

        private static readonly Among[] a_35 = new[]
        {
            new Among("αγαμε", -1, 1, 0),
            new Among("ηκαμε", -1, 1, 0),
            new Among("ηθηκαμε", 1, 1, 0),
            new Among("ησαμε", -1, 1, 0),
            new Among("ουσαμε", -1, 1, 0)
        };

        private static readonly Among[] a_36 = new[]
        {
            new Among("βουβ", -1, 1, 0),
            new Among("ξεθ", -1, 1, 0),
            new Among("πεθ", -1, 1, 0),
            new Among("αποθ", -1, 1, 0),
            new Among("αποκ", -1, 1, 0),
            new Among("ουλ", -1, 1, 0),
            new Among("αναπ", -1, 1, 0),
            new Among("πικρ", -1, 1, 0),
            new Among("ποτ", -1, 1, 0),
            new Among("αποστ", -1, 1, 0),
            new Among("χ", -1, 1, 0),
            new Among("σιχ", 10, 1, 0)
        };

        private static readonly Among[] a_37 = new[]
        {
            new Among("τρ", -1, 1, 0),
            new Among("τσ", -1, 1, 0)
        };

        private static readonly Among[] a_38 = new[]
        {
            new Among("αγανε", -1, 1, 0),
            new Among("ηκανε", -1, 1, 0),
            new Among("ηθηκανε", 1, 1, 0),
            new Among("ησανε", -1, 1, 0),
            new Among("ουσανε", -1, 1, 0),
            new Among("οντανε", -1, 1, 0),
            new Among("ιοντανε", 5, 1, 0),
            new Among("ουντανε", -1, 1, 0),
            new Among("ιουντανε", 7, 1, 0),
            new Among("οτανε", -1, 1, 0),
            new Among("ιοτανε", 9, 1, 0)
        };

        private static readonly Among[] a_39 = new[]
        {
            new Among("ταβ", -1, 1, 0),
            new Among("νταβ", 0, 1, 0),
            new Among("ψηλοταβ", 0, 1, 0),
            new Among("λιβ", -1, 1, 0),
            new Among("κλιβ", 3, 1, 0),
            new Among("ξηροκλιβ", 4, 1, 0),
            new Among("γ", -1, 1, 0),
            new Among("αγ", 6, 1, 0),
            new Among("τραγ", 7, 1, 0),
            new Among("τσαγ", 7, 1, 0),
            new Among("αθιγγ", 6, 1, 0),
            new Among("τσιγγ", 6, 1, 0),
            new Among("ατσιγγ", 11, 1, 0),
            new Among("στεγ", 6, 1, 0),
            new Among("απηγ", 6, 1, 0),
            new Among("σιγ", 6, 1, 0),
            new Among("ανοργ", 6, 1, 0),
            new Among("ενοργ", 6, 1, 0),
            new Among("καλπουζ", -1, 1, 0),
            new Among("θ", -1, 1, 0),
            new Among("μωαμεθ", 19, 1, 0),
            new Among("πιθ", 19, 1, 0),
            new Among("απιθ", 21, 1, 0),
            new Among("δεκ", -1, 1, 0),
            new Among("πελεκ", -1, 1, 0),
            new Among("ικ", -1, 1, 0),
            new Among("ανικ", 25, 1, 0),
            new Among("βουλκ", -1, 1, 0),
            new Among("βασκ", -1, 1, 0),
            new Among("βραχυκ", -1, 1, 0),
            new Among("γαλ", -1, 1, 0),
            new Among("καταγαλ", 30, 1, 0),
            new Among("ολογαλ", 30, 1, 0),
            new Among("βαθυγαλ", 30, 1, 0),
            new Among("μελ", -1, 1, 0),
            new Among("καστελ", -1, 1, 0),
            new Among("πορτολ", -1, 1, 0),
            new Among("πλ", -1, 1, 0),
            new Among("διπλ", 37, 1, 0),
            new Among("λαοπλ", 37, 1, 0),
            new Among("ψυχοπλ", 37, 1, 0),
            new Among("ουλ", -1, 1, 0),
            new Among("μ", -1, 1, 0),
            new Among("ολιγοδαμ", 42, 1, 0),
            new Among("μουσουλμ", 42, 1, 0),
            new Among("δραδουμ", 42, 1, 0),
            new Among("βραχμ", 42, 1, 0),
            new Among("ν", -1, 1, 0),
            new Among("αμερικαν", 47, 1, 0),
            new Among("π", -1, 1, 0),
            new Among("αδαπ", 49, 1, 0),
            new Among("χαμηλοδαπ", 49, 1, 0),
            new Among("πολυδαπ", 49, 1, 0),
            new Among("κοπ", 49, 1, 0),
            new Among("υποκοπ", 53, 1, 0),
            new Among("τσοπ", 49, 1, 0),
            new Among("σπ", 49, 1, 0),
            new Among("ερ", -1, 1, 0),
            new Among("γερ", 57, 1, 0),
            new Among("βετερ", 57, 1, 0),
            new Among("λουθηρ", -1, 1, 0),
            new Among("κορμορ", -1, 1, 0),
            new Among("περιτρ", -1, 1, 0),
            new Among("ουρ", -1, 1, 0),
            new Among("σ", -1, 1, 0),
            new Among("βασ", 64, 1, 0),
            new Among("πολισ", 64, 1, 0),
            new Among("σαρακατσ", 64, 1, 0),
            new Among("θυσ", 64, 1, 0),
            new Among("διατ", -1, 1, 0),
            new Among("πλατ", -1, 1, 0),
            new Among("τσαρλατ", -1, 1, 0),
            new Among("τετ", -1, 1, 0),
            new Among("πουριτ", -1, 1, 0),
            new Among("σουλτ", -1, 1, 0),
            new Among("μαιντ", -1, 1, 0),
            new Among("ζωντ", -1, 1, 0),
            new Among("καστ", -1, 1, 0),
            new Among("φ", -1, 1, 0),
            new Among("διαφ", 78, 1, 0),
            new Among("στεφ", 78, 1, 0),
            new Among("φωτοστεφ", 80, 1, 0),
            new Among("περηφ", 78, 1, 0),
            new Among("υπερηφ", 82, 1, 0),
            new Among("κοιλαρφ", 78, 1, 0),
            new Among("πενταρφ", 78, 1, 0),
            new Among("ορφ", 78, 1, 0),
            new Among("χ", -1, 1, 0),
            new Among("αμηχ", 87, 1, 0),
            new Among("βιομηχ", 87, 1, 0),
            new Among("μεγλοβιομηχ", 89, 1, 0),
            new Among("καπνοβιομηχ", 89, 1, 0),
            new Among("μικροβιομηχ", 89, 1, 0),
            new Among("πολυμηχ", 87, 1, 0),
            new Among("λιχ", 87, 1, 0)
        };

        private static readonly Among[] a_40 = new[]
        {
            new Among("ενδ", -1, 1, 0),
            new Among("συνδ", -1, 1, 0),
            new Among("οδ", -1, 1, 0),
            new Among("διαθ", -1, 1, 0),
            new Among("καθ", -1, 1, 0),
            new Among("ραθ", -1, 1, 0),
            new Among("ταθ", -1, 1, 0),
            new Among("τιθ", -1, 1, 0),
            new Among("εκθ", -1, 1, 0),
            new Among("ενθ", -1, 1, 0),
            new Among("συνθ", -1, 1, 0),
            new Among("ροθ", -1, 1, 0),
            new Among("υπερθ", -1, 1, 0),
            new Among("σθ", -1, 1, 0),
            new Among("ευθ", -1, 1, 0),
            new Among("αρκ", -1, 1, 0),
            new Among("ωφελ", -1, 1, 0),
            new Among("βολ", -1, 1, 0),
            new Among("αιν", -1, 1, 0),
            new Among("πον", -1, 1, 0),
            new Among("ρον", -1, 1, 0),
            new Among("συν", -1, 1, 0),
            new Among("βαρ", -1, 1, 0),
            new Among("βρ", -1, 1, 0),
            new Among("αιρ", -1, 1, 0),
            new Among("φορ", -1, 1, 0),
            new Among("ευρ", -1, 1, 0),
            new Among("πυρ", -1, 1, 0),
            new Among("χωρ", -1, 1, 0),
            new Among("νετ", -1, 1, 0),
            new Among("σχ", -1, 1, 0)
        };

        private static readonly Among[] a_41 = new[]
        {
            new Among("παγ", -1, 1, 0),
            new Among("δ", -1, 1, 0),
            new Among("αδ", 1, 1, 0),
            new Among("θ", -1, 1, 0),
            new Among("αθ", 3, 1, 0),
            new Among("τοκ", -1, 1, 0),
            new Among("σκ", -1, 1, 0),
            new Among("παρακαλ", -1, 1, 0),
            new Among("σκελ", -1, 1, 0),
            new Among("απλ", -1, 1, 0),
            new Among("εμ", -1, 1, 0),
            new Among("αν", -1, 1, 0),
            new Among("βεν", -1, 1, 0),
            new Among("βαρον", -1, 1, 0),
            new Among("κοπ", -1, 1, 0),
            new Among("σερπ", -1, 1, 0),
            new Among("αβαρ", -1, 1, 0),
            new Among("εναρ", -1, 1, 0),
            new Among("αβρ", -1, 1, 0),
            new Among("μπορ", -1, 1, 0),
            new Among("θαρρ", -1, 1, 0),
            new Among("ντρ", -1, 1, 0),
            new Among("υ", -1, 1, 0),
            new Among("νιφ", -1, 1, 0),
            new Among("συρφ", -1, 1, 0)
        };

        private static readonly Among[] a_42 = new[]
        {
            new Among("οντασ", -1, 1, 0),
            new Among("ωντασ", -1, 1, 0)
        };

        private static readonly Among[] a_43 = new[]
        {
            new Among("ομαστε", -1, 1, 0),
            new Among("ιομαστε", 0, 1, 0)
        };

        private static readonly Among[] a_44 = new[]
        {
            new Among("π", -1, 1, 0),
            new Among("απ", 0, 1, 0),
            new Among("ακαταπ", 1, 1, 0),
            new Among("συμπ", 0, 1, 0),
            new Among("ασυμπ", 3, 1, 0),
            new Among("αμεταμφ", -1, 1, 0)
        };

        private static readonly Among[] a_45 = new[]
        {
            new Among("ζ", -1, 1, 0),
            new Among("αλ", -1, 1, 0),
            new Among("παρακαλ", 1, 1, 0),
            new Among("εκτελ", -1, 1, 0),
            new Among("μ", -1, 1, 0),
            new Among("ξ", -1, 1, 0),
            new Among("προ", -1, 1, 0),
            new Among("αρ", -1, 1, 0),
            new Among("νισ", -1, 1, 0)
        };

        private static readonly Among[] a_46 = new[]
        {
            new Among("ηθηκα", -1, 1, 0),
            new Among("ηθηκε", -1, 1, 0),
            new Among("ηθηκεσ", -1, 1, 0)
        };

        private static readonly Among[] a_47 = new[]
        {
            new Among("πιθ", -1, 1, 0),
            new Among("οθ", -1, 1, 0),
            new Among("ναρθ", -1, 1, 0),
            new Among("σκουλ", -1, 1, 0),
            new Among("σκωλ", -1, 1, 0),
            new Among("σφ", -1, 1, 0)
        };

        private static readonly Among[] a_48 = new[]
        {
            new Among("θ", -1, 1, 0),
            new Among("διαθ", 0, 1, 0),
            new Among("παρακαταθ", 0, 1, 0),
            new Among("συνθ", 0, 1, 0),
            new Among("προσθ", 0, 1, 0)
        };

        private static readonly Among[] a_49 = new[]
        {
            new Among("ηκα", -1, 1, 0),
            new Among("ηκε", -1, 1, 0),
            new Among("ηκεσ", -1, 1, 0)
        };

        private static readonly Among[] a_50 = new[]
        {
            new Among("φαγ", -1, 1, 0),
            new Among("ληγ", -1, 1, 0),
            new Among("φρυδ", -1, 1, 0),
            new Among("μαντιλ", -1, 1, 0),
            new Among("μαλλ", -1, 1, 0),
            new Among("ομ", -1, 1, 0),
            new Among("βλεπ", -1, 1, 0),
            new Among("ποδαρ", -1, 1, 0),
            new Among("κυματ", -1, 1, 0),
            new Among("πρωτ", -1, 1, 0),
            new Among("λαχ", -1, 1, 0),
            new Among("πανταχ", -1, 1, 0)
        };

        private static readonly Among[] a_51 = new[]
        {
            new Among("τσα", -1, 1, 0),
            new Among("χαδ", -1, 1, 0),
            new Among("μεδ", -1, 1, 0),
            new Among("λαμπιδ", -1, 1, 0),
            new Among("δε", -1, 1, 0),
            new Among("πλε", -1, 1, 0),
            new Among("μεσαζ", -1, 1, 0),
            new Among("δεσποζ", -1, 1, 0),
            new Among("αιθ", -1, 1, 0),
            new Among("φαρμακ", -1, 1, 0),
            new Among("αγκ", -1, 1, 0),
            new Among("ανηκ", -1, 1, 0),
            new Among("λ", -1, 1, 0),
            new Among("μ", -1, 1, 0),
            new Among("αμ", 13, 1, 0),
            new Among("βρομ", 13, 1, 0),
            new Among("υποτειν", -1, 1, 0),
            new Among("εκλιπ", -1, 1, 0),
            new Among("ρ", -1, 1, 0),
            new Among("ενδιαφερ", 18, 1, 0),
            new Among("αναρρ", 18, 1, 0),
            new Among("πατ", -1, 1, 0),
            new Among("καθαρευ", -1, 1, 0),
            new Among("δευτερευ", -1, 1, 0),
            new Among("λεχ", -1, 1, 0)
        };

        private static readonly Among[] a_52 = new[]
        {
            new Among("ουσα", -1, 1, 0),
            new Among("ουσε", -1, 1, 0),
            new Among("ουσεσ", -1, 1, 0)
        };

        private static readonly Among[] a_53 = new[]
        {
            new Among("πελ", -1, 1, 0),
            new Among("λλ", -1, 1, 0),
            new Among("σμην", -1, 1, 0),
            new Among("ρπ", -1, 1, 0),
            new Among("πρ", -1, 1, 0),
            new Among("φρ", -1, 1, 0),
            new Among("χορτ", -1, 1, 0),
            new Among("οφ", -1, 1, 0),
            new Among("ψοφ", 7, -1, 0),
            new Among("σφ", -1, 1, 0),
            new Among("λοχ", -1, 1, 0),
            new Among("ναυλοχ", 10, -1, 0)
        };

        private static readonly Among[] a_54 = new[]
        {
            new Among("αμαλλι", -1, 1, 0),
            new Among("λ", -1, 1, 0),
            new Among("αμαλ", 1, 1, 0),
            new Among("μ", -1, 1, 0),
            new Among("ουλαμ", 3, 1, 0),
            new Among("εν", -1, 1, 0),
            new Among("δερβεν", 5, 1, 0),
            new Among("π", -1, 1, 0),
            new Among("αειπ", 7, 1, 0),
            new Among("αρτιπ", 7, 1, 0),
            new Among("συμπ", 7, 1, 0),
            new Among("νεοπ", 7, 1, 0),
            new Among("κροκαλοπ", 7, 1, 0),
            new Among("ολοπ", 7, 1, 0),
            new Among("προσωποπ", 7, 1, 0),
            new Among("σιδηροπ", 7, 1, 0),
            new Among("δροσοπ", 7, 1, 0),
            new Among("ασπ", 7, 1, 0),
            new Among("ανυπ", 7, 1, 0),
            new Among("ρ", -1, 1, 0),
            new Among("ασπαρ", 19, 1, 0),
            new Among("χαρ", 19, 1, 0),
            new Among("αχαρ", 21, 1, 0),
            new Among("απερ", 19, 1, 0),
            new Among("τρ", 19, 1, 0),
            new Among("ουρ", 19, 1, 0),
            new Among("τ", -1, 1, 0),
            new Among("διατ", 26, 1, 0),
            new Among("επιτ", 26, 1, 0),
            new Among("συντ", 26, 1, 0),
            new Among("ομοτ", 26, 1, 0),
            new Among("νομοτ", 30, 1, 0),
            new Among("αποτ", 26, 1, 0),
            new Among("υποτ", 26, 1, 0),
            new Among("αβαστ", 26, 1, 0),
            new Among("αιμοστ", 26, 1, 0),
            new Among("προστ", 26, 1, 0),
            new Among("ανυστ", 26, 1, 0),
            new Among("ναυ", -1, 1, 0),
            new Among("αφ", -1, 1, 0),
            new Among("ξεφ", -1, 1, 0),
            new Among("αδηφ", -1, 1, 0),
            new Among("παμφ", -1, 1, 0),
            new Among("πολυφ", -1, 1, 0)
        };

        private static readonly Among[] a_55 = new[]
        {
            new Among("αγα", -1, 1, 0),
            new Among("αγε", -1, 1, 0),
            new Among("αγεσ", -1, 1, 0)
        };

        private static readonly Among[] a_56 = new[]
        {
            new Among("ησα", -1, 1, 0),
            new Among("ησε", -1, 1, 0),
            new Among("ησου", -1, 1, 0)
        };

        private static readonly Among[] a_57 = new[]
        {
            new Among("ν", -1, 1, 0),
            new Among("δωδεκαν", 0, 1, 0),
            new Among("επταν", 0, 1, 0),
            new Among("μεγαλον", 0, 1, 0),
            new Among("ερημον", 0, 1, 0),
            new Among("χερσον", 0, 1, 0)
        };

        private static readonly Among[] a_58 = new[]
        {
            new Among("σβ", -1, 1, 0),
            new Among("ασβ", 0, 1, 0),
            new Among("απλ", -1, 1, 0),
            new Among("αειμν", -1, 1, 0),
            new Among("χρ", -1, 1, 0),
            new Among("αχρ", 4, 1, 0),
            new Among("κοινοχρ", 4, 1, 0),
            new Among("δυσχρ", 4, 1, 0),
            new Among("ευχρ", 4, 1, 0),
            new Among("παλιμψ", -1, 1, 0)
        };

        private static readonly Among[] a_59 = new[]
        {
            new Among("ουνε", -1, 1, 0),
            new Among("ηθουνε", 0, 1, 0),
            new Among("ησουνε", 0, 1, 0)
        };

        private static readonly Among[] a_60 = new[]
        {
            new Among("σπι", -1, 1, 0),
            new Among("ν", -1, 1, 0),
            new Among("εξων", 1, 1, 0),
            new Among("ρ", -1, 1, 0),
            new Among("στραβομουτσ", -1, 1, 0),
            new Among("κακομουτσ", -1, 1, 0)
        };

        private static readonly Among[] a_61 = new[]
        {
            new Among("ουμε", -1, 1, 0),
            new Among("ηθουμε", 0, 1, 0),
            new Among("ησουμε", 0, 1, 0)
        };

        private static readonly Among[] a_62 = new[]
        {
            new Among("αζ", -1, 1, 0),
            new Among("ωριοπλ", -1, 1, 0),
            new Among("ασουσ", -1, 1, 0),
            new Among("παρασουσ", 2, 1, 0),
            new Among("αλλοσουσ", -1, 1, 0),
            new Among("φ", -1, 1, 0),
            new Among("χ", -1, 1, 0)
        };

        private static readonly Among[] a_63 = new[]
        {
            new Among("ματα", -1, 1, 0),
            new Among("ματων", -1, 1, 0),
            new Among("ματοσ", -1, 1, 0)
        };

        private static readonly Among[] a_64 = new[]
        {
            new Among("α", -1, 1, 0),
            new Among("ιουμα", 0, 1, 0),
            new Among("ομουνα", 0, 1, 0),
            new Among("ιομουνα", 2, 1, 0),
            new Among("οσουνα", 0, 1, 0),
            new Among("ιοσουνα", 4, 1, 0),
            new Among("ε", -1, 1, 0),
            new Among("αγατε", 6, 1, 0),
            new Among("ηκατε", 6, 1, 0),
            new Among("ηθηκατε", 8, 1, 0),
            new Among("ησατε", 6, 1, 0),
            new Among("ουσατε", 6, 1, 0),
            new Among("ειτε", 6, 1, 0),
            new Among("ηθειτε", 12, 1, 0),
            new Among("ιεμαστε", 6, 1, 0),
            new Among("ουμαστε", 6, 1, 0),
            new Among("ιουμαστε", 15, 1, 0),
            new Among("ιεσαστε", 6, 1, 0),
            new Among("οσαστε", 6, 1, 0),
            new Among("ιοσαστε", 18, 1, 0),
            new Among("η", -1, 1, 0),
            new Among("ι", -1, 1, 0),
            new Among("αμαι", 21, 1, 0),
            new Among("ιεμαι", 21, 1, 0),
            new Among("ομαι", 21, 1, 0),
            new Among("ουμαι", 21, 1, 0),
            new Among("ασαι", 21, 1, 0),
            new Among("εσαι", 21, 1, 0),
            new Among("ιεσαι", 27, 1, 0),
            new Among("αται", 21, 1, 0),
            new Among("εται", 21, 1, 0),
            new Among("ιεται", 30, 1, 0),
            new Among("ονται", 21, 1, 0),
            new Among("ουνται", 21, 1, 0),
            new Among("ιουνται", 33, 1, 0),
            new Among("ει", 21, 1, 0),
            new Among("αει", 35, 1, 0),
            new Among("ηθει", 35, 1, 0),
            new Among("ησει", 35, 1, 0),
            new Among("οι", 21, 1, 0),
            new Among("αν", -1, 1, 0),
            new Among("αγαν", 40, 1, 0),
            new Among("ηκαν", 40, 1, 0),
            new Among("ηθηκαν", 42, 1, 0),
            new Among("ησαν", 40, 1, 0),
            new Among("ουσαν", 40, 1, 0),
            new Among("οντουσαν", 45, 1, 0),
            new Among("ιοντουσαν", 46, 1, 0),
            new Among("ονταν", 40, 1, 0),
            new Among("ιονταν", 48, 1, 0),
            new Among("ουνταν", 40, 1, 0),
            new Among("ιουνταν", 50, 1, 0),
            new Among("οταν", 40, 1, 0),
            new Among("ιοταν", 52, 1, 0),
            new Among("ομασταν", 40, 1, 0),
            new Among("ιομασταν", 54, 1, 0),
            new Among("οσασταν", 40, 1, 0),
            new Among("ιοσασταν", 56, 1, 0),
            new Among("ουν", -1, 1, 0),
            new Among("ηθουν", 58, 1, 0),
            new Among("ομουν", 58, 1, 0),
            new Among("ιομουν", 60, 1, 0),
            new Among("ησουν", 58, 1, 0),
            new Among("οσουν", 58, 1, 0),
            new Among("ιοσουν", 63, 1, 0),
            new Among("ων", -1, 1, 0),
            new Among("ηδων", 65, 1, 0),
            new Among("ο", -1, 1, 0),
            new Among("ασ", -1, 1, 0),
            new Among("εσ", -1, 1, 0),
            new Among("ηδεσ", 69, 1, 0),
            new Among("ησεσ", 69, 1, 0),
            new Among("ησ", -1, 1, 0),
            new Among("εισ", -1, 1, 0),
            new Among("ηθεισ", 73, 1, 0),
            new Among("οσ", -1, 1, 0),
            new Among("υσ", -1, 1, 0),
            new Among("ουσ", 76, 1, 0),
            new Among("υ", -1, 1, 0),
            new Among("ου", 78, 1, 0),
            new Among("ω", -1, 1, 0),
            new Among("αω", 80, 1, 0),
            new Among("ηθω", 80, 1, 0),
            new Among("ησω", 80, 1, 0)
        };

        private static readonly Among[] a_65 = new[]
        {
            new Among("οτερ", -1, 1, 0),
            new Among("εστερ", -1, 1, 0),
            new Among("υτερ", -1, 1, 0),
            new Among("ωτερ", -1, 1, 0),
            new Among("οτατ", -1, 1, 0),
            new Among("εστατ", -1, 1, 0),
            new Among("υτατ", -1, 1, 0),
            new Among("ωτατ", -1, 1, 0)
        };


        private bool r_has_min_length()
        {
            return current.Length >= 3;
        }

        private bool r_tolower()
        {
            int among_var;
            while (true)
            {
                int c1 = limit - cursor;
                ket = cursor;
                among_var = find_among_b(a_0, null);
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("α");
                        break;
                    }
                    case 2: {
                        slice_from("β");
                        break;
                    }
                    case 3: {
                        slice_from("γ");
                        break;
                    }
                    case 4: {
                        slice_from("δ");
                        break;
                    }
                    case 5: {
                        slice_from("ε");
                        break;
                    }
                    case 6: {
                        slice_from("ζ");
                        break;
                    }
                    case 7: {
                        slice_from("η");
                        break;
                    }
                    case 8: {
                        slice_from("θ");
                        break;
                    }
                    case 9: {
                        slice_from("ι");
                        break;
                    }
                    case 10: {
                        slice_from("κ");
                        break;
                    }
                    case 11: {
                        slice_from("λ");
                        break;
                    }
                    case 12: {
                        slice_from("μ");
                        break;
                    }
                    case 13: {
                        slice_from("ν");
                        break;
                    }
                    case 14: {
                        slice_from("ξ");
                        break;
                    }
                    case 15: {
                        slice_from("ο");
                        break;
                    }
                    case 16: {
                        slice_from("π");
                        break;
                    }
                    case 17: {
                        slice_from("ρ");
                        break;
                    }
                    case 18: {
                        slice_from("σ");
                        break;
                    }
                    case 19: {
                        slice_from("τ");
                        break;
                    }
                    case 20: {
                        slice_from("υ");
                        break;
                    }
                    case 21: {
                        slice_from("φ");
                        break;
                    }
                    case 22: {
                        slice_from("χ");
                        break;
                    }
                    case 23: {
                        slice_from("ψ");
                        break;
                    }
                    case 24: {
                        slice_from("ω");
                        break;
                    }
                    case 25: {
                        if (cursor <= limit_backward)
                        {
                            goto lab0;
                        }
                        cursor--;
                        break;
                    }
                }
                continue;
            lab0: ;
                cursor = limit - c1;
                break;
            }
            return true;
        }

        private bool r_step_1()
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
                    slice_from("φα");
                    break;
                }
                case 2: {
                    slice_from("σκα");
                    break;
                }
                case 3: {
                    slice_from("ολο");
                    break;
                }
                case 4: {
                    slice_from("σο");
                    break;
                }
                case 5: {
                    slice_from("τατο");
                    break;
                }
                case 6: {
                    slice_from("κρε");
                    break;
                }
                case 7: {
                    slice_from("περ");
                    break;
                }
                case 8: {
                    slice_from("τερ");
                    break;
                }
                case 9: {
                    slice_from("φω");
                    break;
                }
                case 10: {
                    slice_from("καθεστ");
                    break;
                }
                case 11: {
                    slice_from("γεγον");
                    break;
                }
            }
            B_test1 = false;
            return true;
        }

        private bool r_step_s1()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_3, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            among_var = find_among_b(a_2, null);
            if (among_var == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    slice_from("ι");
                    break;
                }
                case 2: {
                    slice_from("ιζ");
                    break;
                }
            }
            return true;
        }

        private bool r_step_s2()
        {
            ket = cursor;
            if (find_among_b(a_5, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_4, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ων");
            return true;
        }

        private bool r_step_s3()
        {
            int among_var;
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("ισα")))
                {
                    goto lab1;
                }
                bra = cursor;
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                slice_from("ισ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
            }
        lab0: ;
            if (find_among_b(a_7, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            among_var = find_among_b(a_6, null);
            if (among_var == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    slice_from("ι");
                    break;
                }
                case 2: {
                    slice_from("ισ");
                    break;
                }
            }
            return true;
        }

        private bool r_step_s4()
        {
            ket = cursor;
            if (find_among_b(a_9, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_8, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ι");
            return true;
        }

        private bool r_step_s5()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_11, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            among_var = find_among_b(a_10, null);
            if (among_var == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            switch (among_var) {
                case 1: {
                    slice_from("ι");
                    break;
                }
                case 2: {
                    slice_from("ιστ");
                    break;
                }
            }
            return true;
        }

        private bool r_step_s6()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_14, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                among_var = find_among_b(a_12, null);
                if (among_var == 0)
                {
                    goto lab1;
                }
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                switch (among_var) {
                    case 1: {
                        slice_from("ισμ");
                        break;
                    }
                    case 2: {
                        slice_from("ι");
                        break;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                among_var = find_among_b(a_13, null);
                if (among_var == 0)
                {
                    return false;
                }
                bra = cursor;
                switch (among_var) {
                    case 1: {
                        slice_from("αγνωστ");
                        break;
                    }
                    case 2: {
                        slice_from("ατομ");
                        break;
                    }
                    case 3: {
                        slice_from("γνωστ");
                        break;
                    }
                    case 4: {
                        slice_from("εθν");
                        break;
                    }
                    case 5: {
                        slice_from("εκλεκτ");
                        break;
                    }
                    case 6: {
                        slice_from("σκεπτ");
                        break;
                    }
                    case 7: {
                        slice_from("τοπ");
                        break;
                    }
                    case 8: {
                        slice_from("αλεξανδρ");
                        break;
                    }
                    case 9: {
                        slice_from("βυζαντ");
                        break;
                    }
                    case 10: {
                        slice_from("θεατρ");
                        break;
                    }
                }
            }
        lab0: ;
            return true;
        }

        private bool r_step_s7()
        {
            ket = cursor;
            if (find_among_b(a_16, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_15, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("αρακ");
            return true;
        }

        private bool r_step_s8()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_18, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                among_var = find_among_b(a_17, null);
                if (among_var == 0)
                {
                    goto lab1;
                }
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                switch (among_var) {
                    case 1: {
                        slice_from("ακ");
                        break;
                    }
                    case 2: {
                        slice_from("ιτσ");
                        break;
                    }
                }
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                bra = cursor;
                if (!(eq_s_b("κορ")))
                {
                    return false;
                }
                slice_from("ιτσ");
            }
        lab0: ;
            return true;
        }

        private bool r_step_s9()
        {
            ket = cursor;
            if (find_among_b(a_21, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_19, null) == 0)
                {
                    goto lab1;
                }
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                slice_from("ιδ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_20, null) == 0)
                {
                    return false;
                }
                slice_from("ιδ");
            }
        lab0: ;
            return true;
        }

        private bool r_step_s10()
        {
            ket = cursor;
            if (find_among_b(a_23, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_22, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ισκ");
            return true;
        }

        private bool r_step_2a()
        {
            ket = cursor;
            if (find_among_b(a_24, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            {
                int c1 = limit - cursor;
                if (find_among_b(a_25, null) == 0)
                {
                    goto lab0;
                }
                return false;
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c = cursor;
                insert(cursor, cursor, "αδ");
                cursor = c;
            }
            return true;
        }

        private bool r_step_2b()
        {
            ket = cursor;
            if (find_among_b(a_26, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_27, null) == 0)
            {
                return false;
            }
            slice_from("εδ");
            return true;
        }

        private bool r_step_2c()
        {
            ket = cursor;
            if (find_among_b(a_28, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_29, null) == 0)
            {
                return false;
            }
            slice_from("ουδ");
            return true;
        }

        private bool r_step_2d()
        {
            ket = cursor;
            if (find_among_b(a_30, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_31, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ε");
            return true;
        }

        private bool r_step_3()
        {
            ket = cursor;
            if (find_among_b(a_32, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (in_grouping_b(g_v, 945, 969, false) != 0)
            {
                return false;
            }
            slice_from("ι");
            return true;
        }

        private bool r_step_4()
        {
            ket = cursor;
            if (find_among_b(a_33, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (in_grouping_b(g_v, 945, 969, false) != 0)
                {
                    goto lab1;
                }
                slice_from("ικ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
            }
        lab0: ;
            bra = cursor;
            if (find_among_b(a_34, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ικ");
            return true;
        }

        private bool r_step_5a()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("αγαμε")))
                {
                    goto lab0;
                }
                bra = cursor;
                if (cursor > limit_backward)
                {
                    goto lab0;
                }
                slice_from("αγαμ");
            lab0: ;
                cursor = limit - c1;
            }
            {
                int c2 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_35, null) == 0)
                {
                    goto lab1;
                }
                bra = cursor;
                slice_del();
                B_test1 = false;
            lab1: ;
                cursor = limit - c2;
            }
            ket = cursor;
            if (!(eq_s_b("αμε")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_36, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("αμ");
            return true;
        }

        private bool r_step_5b()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_38, null) == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                slice_del();
                B_test1 = false;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_37, null) == 0)
                {
                    goto lab0;
                }
                if (cursor > limit_backward)
                {
                    goto lab0;
                }
                slice_from("αγαν");
            lab0: ;
                cursor = limit - c1;
            }
            ket = cursor;
            if (!(eq_s_b("ανε")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c2 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (in_grouping_b(g_v2, 945, 969, false) != 0)
                {
                    goto lab2;
                }
                slice_from("αν");
                goto lab1;
            lab2: ;
                cursor = limit - c2;
                ket = cursor;
            }
        lab1: ;
            bra = cursor;
            if (find_among_b(a_39, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("αν");
            return true;
        }

        private bool r_step_5c()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("ησετε")))
                {
                    goto lab0;
                }
                bra = cursor;
                slice_del();
                B_test1 = false;
            lab0: ;
                cursor = limit - c1;
            }
            ket = cursor;
            if (!(eq_s_b("ετε")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c2 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (in_grouping_b(g_v2, 945, 969, false) != 0)
                {
                    goto lab2;
                }
                slice_from("ετ");
                goto lab1;
            lab2: ;
                cursor = limit - c2;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_40, null) == 0)
                {
                    goto lab3;
                }
                slice_from("ετ");
                goto lab1;
            lab3: ;
                cursor = limit - c2;
                ket = cursor;
            }
        lab1: ;
            bra = cursor;
            if (find_among_b(a_41, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ετ");
            return true;
        }

        private bool r_step_5d()
        {
            ket = cursor;
            if (find_among_b(a_42, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (!(eq_s_b("αρχ")))
                {
                    goto lab1;
                }
                if (cursor > limit_backward)
                {
                    goto lab1;
                }
                slice_from("οντ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                bra = cursor;
                if (!(eq_s_b("κρε")))
                {
                    return false;
                }
                slice_from("ωντ");
            }
        lab0: ;
            return true;
        }

        private bool r_step_5e()
        {
            ket = cursor;
            if (find_among_b(a_43, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (!(eq_s_b("ον")))
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ομαστ");
            return true;
        }

        private bool r_step_5f()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (!(eq_s_b("ιεστε")))
                {
                    goto lab0;
                }
                bra = cursor;
                slice_del();
                B_test1 = false;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_44, null) == 0)
                {
                    goto lab0;
                }
                if (cursor > limit_backward)
                {
                    goto lab0;
                }
                slice_from("ιεστ");
            lab0: ;
                cursor = limit - c1;
            }
            ket = cursor;
            if (!(eq_s_b("εστε")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_45, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ιεστ");
            return true;
        }

        private bool r_step_5g()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_46, null) == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                slice_del();
                B_test1 = false;
            lab0: ;
                cursor = limit - c1;
            }
            ket = cursor;
            if (find_among_b(a_49, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c2 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_47, null) == 0)
                {
                    goto lab2;
                }
                slice_from("ηκ");
                goto lab1;
            lab2: ;
                cursor = limit - c2;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_48, null) == 0)
                {
                    return false;
                }
                if (cursor > limit_backward)
                {
                    return false;
                }
                slice_from("ηκ");
            }
        lab1: ;
            return true;
        }

        private bool r_step_5h()
        {
            ket = cursor;
            if (find_among_b(a_52, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_50, null) == 0)
                {
                    goto lab1;
                }
                slice_from("ουσ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                ket = cursor;
                bra = cursor;
                if (find_among_b(a_51, null) == 0)
                {
                    return false;
                }
                if (cursor > limit_backward)
                {
                    return false;
                }
                slice_from("ουσ");
            }
        lab0: ;
            return true;
        }

        private bool r_step_5i()
        {
            int among_var;
            ket = cursor;
            if (find_among_b(a_55, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            {
                int c1 = limit - cursor;
                ket = cursor;
                bra = cursor;
                if (!(eq_s_b("κολλ")))
                {
                    goto lab1;
                }
                slice_from("αγ");
                goto lab0;
            lab1: ;
                cursor = limit - c1;
                {
                    int c2 = limit - cursor;
                    ket = cursor;
                    bra = cursor;
                    among_var = find_among_b(a_53, null);
                    if (among_var == 0)
                    {
                        goto lab3;
                    }
                    switch (among_var) {
                        case 1: {
                            slice_from("αγ");
                            break;
                        }
                    }
                    goto lab2;
                lab3: ;
                    cursor = limit - c2;
                    ket = cursor;
                    bra = cursor;
                    if (find_among_b(a_54, null) == 0)
                    {
                        return false;
                    }
                    if (cursor > limit_backward)
                    {
                        return false;
                    }
                    slice_from("αγ");
                }
            lab2: ;
            }
        lab0: ;
            return true;
        }

        private bool r_step_5j()
        {
            ket = cursor;
            if (find_among_b(a_56, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_57, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ησ");
            return true;
        }

        private bool r_step_5k()
        {
            ket = cursor;
            if (!(eq_s_b("ηστε")))
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_58, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ηστ");
            return true;
        }

        private bool r_step_5l()
        {
            ket = cursor;
            if (find_among_b(a_59, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_60, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ουν");
            return true;
        }

        private bool r_step_5m()
        {
            ket = cursor;
            if (find_among_b(a_61, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            B_test1 = false;
            ket = cursor;
            bra = cursor;
            if (find_among_b(a_62, null) == 0)
            {
                return false;
            }
            if (cursor > limit_backward)
            {
                return false;
            }
            slice_from("ουμ");
            return true;
        }

        private bool r_step_6()
        {
            {
                int c1 = limit - cursor;
                ket = cursor;
                if (find_among_b(a_63, null) == 0)
                {
                    goto lab0;
                }
                bra = cursor;
                slice_from("μα");
            lab0: ;
                cursor = limit - c1;
            }
            if (!B_test1)
            {
                return false;
            }
            ket = cursor;
            if (find_among_b(a_64, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        private bool r_step_7()
        {
            ket = cursor;
            if (find_among_b(a_65, null) == 0)
            {
                return false;
            }
            bra = cursor;
            slice_del();
            return true;
        }

        protected override bool stem()
        {
            limit_backward = cursor;
            cursor = limit;
            {
                int c1 = limit - cursor;
                r_tolower();
                cursor = limit - c1;
            }
            if (!r_has_min_length())
                return false;
            B_test1 = true;
            {
                int c2 = limit - cursor;
                r_step_1();
                cursor = limit - c2;
            }
            {
                int c3 = limit - cursor;
                r_step_s1();
                cursor = limit - c3;
            }
            {
                int c4 = limit - cursor;
                r_step_s2();
                cursor = limit - c4;
            }
            {
                int c5 = limit - cursor;
                r_step_s3();
                cursor = limit - c5;
            }
            {
                int c6 = limit - cursor;
                r_step_s4();
                cursor = limit - c6;
            }
            {
                int c7 = limit - cursor;
                r_step_s5();
                cursor = limit - c7;
            }
            {
                int c8 = limit - cursor;
                r_step_s6();
                cursor = limit - c8;
            }
            {
                int c9 = limit - cursor;
                r_step_s7();
                cursor = limit - c9;
            }
            {
                int c10 = limit - cursor;
                r_step_s8();
                cursor = limit - c10;
            }
            {
                int c11 = limit - cursor;
                r_step_s9();
                cursor = limit - c11;
            }
            {
                int c12 = limit - cursor;
                r_step_s10();
                cursor = limit - c12;
            }
            {
                int c13 = limit - cursor;
                r_step_2a();
                cursor = limit - c13;
            }
            {
                int c14 = limit - cursor;
                r_step_2b();
                cursor = limit - c14;
            }
            {
                int c15 = limit - cursor;
                r_step_2c();
                cursor = limit - c15;
            }
            {
                int c16 = limit - cursor;
                r_step_2d();
                cursor = limit - c16;
            }
            {
                int c17 = limit - cursor;
                r_step_3();
                cursor = limit - c17;
            }
            {
                int c18 = limit - cursor;
                r_step_4();
                cursor = limit - c18;
            }
            {
                int c19 = limit - cursor;
                r_step_5a();
                cursor = limit - c19;
            }
            {
                int c20 = limit - cursor;
                r_step_5b();
                cursor = limit - c20;
            }
            {
                int c21 = limit - cursor;
                r_step_5c();
                cursor = limit - c21;
            }
            {
                int c22 = limit - cursor;
                r_step_5d();
                cursor = limit - c22;
            }
            {
                int c23 = limit - cursor;
                r_step_5e();
                cursor = limit - c23;
            }
            {
                int c24 = limit - cursor;
                r_step_5f();
                cursor = limit - c24;
            }
            {
                int c25 = limit - cursor;
                r_step_5g();
                cursor = limit - c25;
            }
            {
                int c26 = limit - cursor;
                r_step_5h();
                cursor = limit - c26;
            }
            {
                int c27 = limit - cursor;
                r_step_5j();
                cursor = limit - c27;
            }
            {
                int c28 = limit - cursor;
                r_step_5i();
                cursor = limit - c28;
            }
            {
                int c29 = limit - cursor;
                r_step_5k();
                cursor = limit - c29;
            }
            {
                int c30 = limit - cursor;
                r_step_5l();
                cursor = limit - c30;
            }
            {
                int c31 = limit - cursor;
                r_step_5m();
                cursor = limit - c31;
            }
            {
                int c32 = limit - cursor;
                r_step_6();
                cursor = limit - c32;
            }
            {
                int c33 = limit - cursor;
                r_step_7();
                cursor = limit - c33;
            }
            cursor = limit_backward;
            return true;
        }

    }
}

