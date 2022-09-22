using LabelsNow.ViewModels;
using System.Collections.Generic;

namespace LabelsNow.Data
{
    public class TestData
    {
        public static List<LabelAddressViewModel> GetTestData()
        {
            List<LabelAddressViewModel> testData = new List<LabelAddressViewModel>();

            for (int i = 0; i < 3; i++)
            {
                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "A Test data",
                    Line2 = "Piedras No 623",
                    Line3 = "Piso2 Dto.4",
                    Line4 = "C1070AAM Capital Federal",
                    Line5 = "ARGENTINA",
                    Line6 = "",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "Mr Test data",
                    Line2 = "200 Broadway Av",
                    Line3 = "WEST BEACH SA 5024",
                    Line4 = "AUSTRALIA",
                    Line5 = "",
                    Line6 = "",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "F Test data OY",
                    Line2 = "Test data Ltd",
                    Line3 = "Albertinkatu 36 B",
                    Line4 = "00180 HELSINKI",
                    Line5 = "FINLAND",
                    Line6 = "",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "G Herrn",
                    Line2 = "Test data",
                    Line3 = "Wittekindshof",
                    Line4 = "Schulstrasse 4",
                    Line5 = "32547 Bad Oyenhausen",
                    Line6 = "GERMANY",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "UK Test data Ltd",
                    Line2 = "Ardenham Court",
                    Line3 = "Oxford Road",
                    Line4 = "AYLESBURY",
                    Line5 = "BUCKINGHAMSHIRE",
                    Line6 = "HP19 3EQ",
                    Line7 = "UNITED KINGDOM",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "I Test data Service",
                    Line2 = "12 Shderot Yerushalayim",
                    Line3 = "68021 Tel Aviv - Yafo",
                    Line4 = "ISRAEL",
                    Line5 = "",
                    Line6 = "",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "AM Test data, S.A. de C.V.",
                    Line2 = "Flexible Test data Division",
                    Line3 = "Via Gustavo Baz No. 166",
                    Line4 = "Col. San Jerónimo Tepetlacalco",
                    Line5 = "54090 Tlalnepantla, Edo. de México",
                    Line6 = "P.O. Box 58 BIS Col. Centro",
                    Line7 = "06600 México, D.F.",
                    Line8 = "MEXICO",
                });


                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "RU Test data",
                    Line2 = "ul. Lesnaya d. 5",
                    Line3 = "pos.Lesnoe locality",
                    Line4 = "ALEKSCEVSKTY r-n",
                    Line5 = "VORONEJSKAYA obl province(oblast)",
                    Line6 = "247112",
                    Line7 = "RUSSIAN FEDERATION",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US1 Test data",
                    Line2 = "2883 Roguski Road",
                    Line3 = "Alexandria",
                    Line4 = "Louisiana",
                    Line5 = "71301",
                    Line6 = "UNITED STATES",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US2 Test data",
                    Line2 = "711 - 2880 Nulla St.",
                    Line3 = "Mankato Mississippi",
                    Line4 = "96522",
                    Line5 = "UNITED STATES",
                    Line6 = "",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US3 Test data",
                    Line2 = "123 Faux St.",
                    Line3 = "Alexandria",
                    Line4 = "VA",
                    Line5 = "22310",
                    Line6 = "UNITED STATES",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US4 Test data",
                    Line2 = "Apt 1X",
                    Line3 = "451 Mystery St.",
                    Line4 = "Trenton",
                    Line5 = "NJ",
                    Line6 = "08601",
                    Line7 = "UNITED STATES",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US5 Test data",
                    Line2 = "1630 Revello Dr.",
                    Line3 = "Sunnydale",
                    Line4 = "CA",
                    Line5 = "95037",
                    Line6 = "UNITED STATES",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US6 Test data",
                    Line2 = "404 Piikoi Street",
                    Line3 = "Honolulu",
                    Line4 = "HI",
                    Line5 = "96813",
                    Line6 = "UNITED STATES",
                    Line7 = "",
                    Line8 = "",
                });

                testData.Add(new LabelAddressViewModel
                {
                    Line1 = "US7 Test data",
                    Line2 = "Apt 5B",
                    Line3 = "129 W. 81 St.",
                    Line4 = "New York",
                    Line5 = "NY",
                    Line6 = "10024 - 7207",
                    Line7 = "UNITED STATES",
                    Line8 = "",
                });
            }

            return testData;
        }
    }
}
