using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Bytescout.Spreadsheet;
using System.IO;
using System.Linq;
using Exon.Recab.Domain.Entity;
using System.Threading;

namespace Exon.Recab.Convert
{
    class Program
    {
        static void Main(string[] args)
        {
            Recab.Domain.SqlServer.SdbContext _sdb = new Domain.SqlServer.SdbContext();

            //  Directory.GetFiles()

            List<string> files = Directory.GetFiles(@"C:\Users\m.abbasi.REKAB\Desktop\khodro - ref").ToList();

            FileStream stream = new FileStream(@"C:\Users\m.abbasi.REKAB\Desktop\dd\Log.txt", mode: FileMode.Append, access: FileAccess.Write);

            stream.Position = stream.Length;
            StreamWriter writer = new StreamWriter(stream);


            foreach (var file in files)
            {
                Spreadsheet document = new Spreadsheet();
                document.LoadFromFile(file);
                Worksheet worksheet = document.Workbook.Worksheets.ByName("Sheet1");


                #region retrive
                for (int i = 1; i < 99; i++)
                {

                    string brandCell = worksheet.Cell(i, 0).ValueAsString.TrimStart(' ');
                    if (brandCell != "" && brandCell != null)
                    {
                        FeatureValue Brand = _sdb.FeatureValues.FirstOrDefault(fv => fv.Title == brandCell);

                        if (Brand == null)
                        {
                            //Thread.Sleep(int.MaxValue);
                            Brand = new FeatureValue { Title = brandCell, CategoryFeatureId = 3 };
                            _sdb.FeatureValues.Add(Brand);
                            _sdb.SaveChanges();

                        }

                        #region model
                        string modelCell = worksheet.Cell(i, 1).ValueAsString.TrimStart(' ');
                        FeatureValue Model = _sdb.FeatureValues.FirstOrDefault(fv => fv.Title == modelCell);

                        if (Model == null)
                        {
                            // Thread.Sleep(int.MaxValue);
                            Model = new FeatureValue { Title = modelCell, Description = Brand.Title, CategoryFeatureId = 4 };
                            _sdb.FeatureValues.Add(Model);
                            _sdb.SaveChanges();

                            Model.ParentList.Add(new FeatureValueDependency { FeatureValueId = Brand.Id });
                            Brand.ChildList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });

                            _sdb.SaveChanges();
                        }

                        #endregion
                        #region sall

                        string SallCell = worksheet.Cell(i, 3).ValueAsString.TrimStart(' ');

                        CategoryFeature CategoryFeature3 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سال تولید");

                        FeatureValue sall = CategoryFeature3.FeatureValueList.FirstOrDefault(fv => fv.Title == SallCell && fv.Description == Model.Title + " " + Model.Description);

                        if (sall == null)
                        {
                            sall = new FeatureValue { Title = SallCell, Description = Model.Title + " " + Model.Description };
                            CategoryFeature3.FeatureValueList.Add(sall);
                            _sdb.SaveChanges();

                            sall.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                            Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = sall.Id });
                        }

                        #endregion

                        #region dandeh

                        string DandehCell = worksheet.Cell(i, 10).ValueAsString.TrimStart(' ');
                        CategoryFeature CategoryFeature10 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "جعبه دنده");

                        FeatureValue Dandeh = CategoryFeature10.FeatureValueList.FirstOrDefault(fv => fv.Title == DandehCell && fv.Description == sall.Title + " " + sall.Description);

                        if (Dandeh == null)
                        {
                            Dandeh = new FeatureValue { Title = DandehCell, Description = sall.Title + " " + sall.Description };
                            CategoryFeature10.FeatureValueList.Add(Dandeh);
                            _sdb.SaveChanges();

                            Dandeh.ParentList.Add(new FeatureValueDependency { FeatureValueId = sall.Id });
                            sall.ChildList.Add(new FeatureValueDependency { FeatureValueId = Dandeh.Id });

                        }

                        #region hajm

                        string hajmCell = worksheet.Cell(i, 24).ValueAsString.TrimStart(' ');
                        CategoryFeature CategoryFeature26 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Id == 49);

                        FeatureValue hajm = CategoryFeature26.FeatureValueList.FirstOrDefault(fv => fv.Title == hajmCell && fv.Description == Dandeh.Title + " " + Dandeh.Description);

                        if (hajm == null)
                        {
                            hajm = new FeatureValue { Title = hajmCell, Description = Dandeh.Title + " " + Dandeh.Description };
                            CategoryFeature26.FeatureValueList.Add(hajm);
                            _sdb.SaveChanges();

                            hajm.ParentList.Add(new FeatureValueDependency { FeatureValueId = Dandeh.Id });
                            Dandeh.ChildList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });

                        }

                        else
                        {
                            Console.WriteLine("prosess FUCK UP");
                            Console.WriteLine(System.Text.Encoding.ASCII.GetString(System.Text.Encoding.UTF8.GetBytes(Dandeh.Description).ToArray()));

                            Console.ReadKey();
                            Console.ReadLine();

                            throw new Exception();
                        }

                        #endregion



                        #endregion



                        for (int j = 2; j < 23; j++)
                        {

                            Cell currentCell = worksheet.Cell(i, j);

                            if (currentCell.ValueAsString != "" && currentCell.ValueAsString != null)
                            {
                                #region
                                switch (j)
                                {

                                    case 2: // شاسی

                                        CategoryFeature CategoryFeature2 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "نوع کلاس");

                                        if (CategoryFeature2 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature2.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature2.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }


                                        }
                                        break;

                                    //case 3: //سال تولید
                                    //    CategoryFeature CategoryFeature3 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سال تولید");

                                    //    if (CategoryFeature3 != null)
                                    //    {
                                    //        FeatureValue FeatureValue = CategoryFeature3.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                    //        if (FeatureValue == null)
                                    //        {
                                    //            FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                    //            CategoryFeature3.FeatureValueList.Add(newFeatureValue);
                                    //            _sdb.SaveChanges();

                                    //            newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //            Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                    //        }
                                    //        else
                                    //        {

                                    //            FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //            Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                    //        }


                                    //    }
                                    //    break;
                                    case 4: // کشور سازنده
                                        CategoryFeature CategoryFeature4 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "کشور سازنده");
                                        if (CategoryFeature4 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature4.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature4.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }

                                        }
                                        break;
                                    case 5:  // سیلندر
                                        CategoryFeature CategoryFeature5 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "تعدادسیلندر");

                                        if (CategoryFeature5 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature5.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature5.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }

                                        }
                                        break;
                                    case 6:  // حجم پیشرانه
                                        CategoryFeature CategoryFeature6 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حجم پیشرانه (سی سی)");
                                        if (CategoryFeature6 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature6.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature6.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;

                                    case 7:  // تعداد سوپاپ
                                        CategoryFeature CategoryFeature7 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "تعداد سوپاپ");
                                        if (CategoryFeature7 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature7.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature7.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;

                                    case 8:  //قدرت پیشرانه
                                        CategoryFeature CategoryFeature8 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "قدرت پیشرانه (اسب بخار)");
                                        if (CategoryFeature8 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature8.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature8.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 9:   //گشتاور پیشرانه
                                        CategoryFeature CategoryFeature9 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "گشتاور پیشرانه (نیوتن متر)");
                                        if (CategoryFeature9 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature9.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature9.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    //case 10:  //جعبه دنده
                                    //    CategoryFeature CategoryFeature10 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "جعبه دنده");
                                    //    if (CategoryFeature10 != null)
                                    //    {
                                    //        FeatureValue FeatureValue = CategoryFeature10.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                    //        if (FeatureValue == null)
                                    //        {
                                    //            FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                    //            CategoryFeature10.FeatureValueList.Add(newFeatureValue);
                                    //            _sdb.SaveChanges();

                                    //            newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //            Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                    //        }
                                    //        else
                                    //        {

                                    //            FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //            Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                    //        }
                                    //    }
                                    //    break;
                                    case 11:  // شتاب صفرتاصد
                                        CategoryFeature CategoryFeature11 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "شتاب صفر تا صد (ثانیه)");
                                        if (CategoryFeature11 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature11.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature11.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 12:  //حداکثر سرعت
                                        CategoryFeature CategoryFeature12 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حداکثر سرعت (کیلومتر)");

                                        if (CategoryFeature12 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature12.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature12.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 13: //میانگین مصرفسوخت
                                        CategoryFeature CategoryFeature13 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "میانگین مصرف سوخت (لیتر در 100 کیلومتر)");
                                        if (CategoryFeature13 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature13.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature13.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 14: //حجم مخزن سوخت
                                        CategoryFeature CategoryFeature14 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حجم مخزن سوخت (لیتر)");
                                        if (CategoryFeature14 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature14.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature14.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 15: //وزن خالص
                                        CategoryFeature CategoryFeature15 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "وزن خالص(کیلوگرم)");
                                        if (CategoryFeature15 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature15.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature15.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 16:  //  طول
                                        CategoryFeature CategoryFeature16 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "طول (میلیمتر)");
                                        if (CategoryFeature16 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature16.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature16.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 17:  //عرض
                                        CategoryFeature CategoryFeature17 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "عرض (میلیمتر)");
                                        if (CategoryFeature17 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature17.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature17.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 18:  //ارتفاع
                                        CategoryFeature CategoryFeature18 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "ارتفاع (میلیمتر)");
                                        if (CategoryFeature18 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature18.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature18.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 19:    //  استاندارد آلایندگی
                                        CategoryFeature CategoryFeature19 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "استاندارد آلایندگی");
                                        if (CategoryFeature19 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature19.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature19.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 20:    //کیسه هوا
                                        CategoryFeature CategoryFeature20 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "کیسه هوا");
                                        if (CategoryFeature20 != null)
                                        {
                                            string data = currentCell.ValueAsString.Replace("/", "-");
                                            data = data.Replace("–", "-");
                                            data = data.Replace("\n\r", "-");
                                            data = data.Replace("\n", "-");
                                            data = data.Replace("\r", "-");
                                            data = data.Replace(",", "-");
                                            data = data.Replace("،", "-");
                                            data = data.Replace("--", "-");
                                            data = data.Replace("_", "-");
                                            // string[] listseperated = data.Split('-');

                                            // foreach (var item in listseperated)
                                            // {
                                            if (data != "" && data != null && data != " " && data != "  ")
                                            {
                                                FeatureValue FeatureValue = CategoryFeature20.FeatureValueList.FirstOrDefault(fv => fv.Title == data);

                                                if (FeatureValue == null)
                                                {
                                                    FeatureValue newFeatureValue = new FeatureValue { Title = data };
                                                    CategoryFeature20.FeatureValueList.Add(newFeatureValue);
                                                    _sdb.SaveChanges();

                                                    newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                                }
                                                else
                                                {

                                                    FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                                }
                                            }
                                            // }
                                        }
                                        break;
                                    case 21:    //ترمز
                                        CategoryFeature CategoryFeature21 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سیستم ترمز");
                                        if (CategoryFeature21 != null)
                                        {
                                            string data = currentCell.ValueAsString.Replace("/", "-");
                                            data = data.Replace("–", "-");
                                            data = data.Replace("\n\r", "-");
                                            data = data.Replace("\n", "-");
                                            data = data.Replace("\r", "-");
                                            data = data.Replace(",", "-");
                                            data = data.Replace("،", "-");
                                            data = data.Replace("--", "-");
                                            data = data.Replace("_", "-");
                                            //string[] listseperated = data.Split('-');

                                            //  foreach (var item in listseperated)
                                            // {

                                            if (data != "" && data != null && data != " " && data != "  ")
                                            {
                                                FeatureValue FeatureValue = CategoryFeature21.FeatureValueList.FirstOrDefault(fv => fv.Title == data);

                                                if (FeatureValue == null)
                                                {
                                                    FeatureValue newFeatureValue = new FeatureValue { Title = data };
                                                    CategoryFeature21.FeatureValueList.Add(newFeatureValue);
                                                    _sdb.SaveChanges();

                                                    newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                                }
                                                else
                                                {

                                                    FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                                }
                                            }
                                            // }
                                        }
                                        break;
                                    case 22:    //  سایر امکانات
                                        CategoryFeature CategoryFeature22 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سایر امکانات");
                                        if (CategoryFeature22 != null)
                                        {
                                            string data = currentCell.ValueAsString.Replace("/", "-");
                                            data = data.Replace("–", "-");
                                            data = data.Replace("\n\r", "-");
                                            data = data.Replace("\n", "-");
                                            data = data.Replace("\r", "-");
                                            data = data.Replace(",", "-");
                                            data = data.Replace("،", "-");
                                            data = data.Replace("--", "-");
                                            data = data.Replace("_", "-");
                                            // string[] listseperated = data.Split('-');

                                            // foreach (var item in listseperated)
                                            // {
                                            if (data != "" && data != null && data != " " && data != "  ")
                                            {
                                                FeatureValue FeatureValue = CategoryFeature22.FeatureValueList.FirstOrDefault(fv => fv.Title == data);

                                                if (FeatureValue == null)
                                                {
                                                    FeatureValue newFeatureValue = new FeatureValue { Title = data };
                                                    CategoryFeature22.FeatureValueList.Add(newFeatureValue);
                                                    _sdb.SaveChanges();

                                                    newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                                }
                                                else
                                                {

                                                    FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = hajm.Id });
                                                    hajm.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                                }
                                            }
                                            // }

                                        }
                                        break;
                                    default:
                                        break;

                                }

                                #endregion
                            }
                            _sdb.SaveChanges();
                        }


                    }
                }
                #endregion

                document.Close();
            }


            writer.Flush();
            writer.Close();

            Console.WriteLine("prosess complete");
            Console.WriteLine("Press any key to continue...");

            Console.ReadKey();

        }




    }
}
