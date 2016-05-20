using Bytescout.Spreadsheet;
using Exon.Recab.Domain.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exon.Recab.Convert.Motor
{
    class Program
    {
        static void Main(string[] args)
        {
            Recab.Domain.SqlServer.SdbContext _sdb = new Domain.SqlServer.SdbContext();

            //  Directory.GetFiles()

            _sdb.Users.ToList();

            List<string> files = Directory.GetFiles(@"E:\BackUP\Review\MOTOR").ToList();

            FileStream stream = new FileStream(@"E:\BackUP\Review\bug\Log.txt", mode: FileMode.Append, access: FileAccess.Write);

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
                        FeatureValue Brand = _sdb.FeatureValues.FirstOrDefault(fv => fv.Title == brandCell && fv.CategoryFeature.CategoryId == 2);

                        if (Brand == null)
                        {
                            //Thread.Sleep(int.MaxValue);
                            Brand = new FeatureValue { Title = brandCell, CategoryFeatureId = 51 };
                            _sdb.FeatureValues.Add(Brand);
                            _sdb.SaveChanges();

                        }

                        #region model
                        string modelCell = worksheet.Cell(i, 1).ValueAsString.TrimStart(' ');
                        FeatureValue Model = _sdb.FeatureValues.FirstOrDefault(fv => fv.Title == modelCell && fv.CategoryFeature.CategoryId == 2 && fv.Description == Brand.Title);

                        if (Model == null)
                        {
                            // Thread.Sleep(int.MaxValue);
                            Model = new FeatureValue { Title = modelCell, Description = Brand.Title, CategoryFeatureId = 52 };
                            _sdb.FeatureValues.Add(Model);
                            _sdb.SaveChanges();

                            Model.ParentList.Add(new FeatureValueDependency { FeatureValueId = Brand.Id });
                            Brand.ChildList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });

                            _sdb.SaveChanges();
                        }
                        else
                        {
                            Model.ParentList.Add(new FeatureValueDependency { FeatureValueId = Brand.Id });
                            Brand.ChildList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });

                            _sdb.SaveChanges();

                        }
                       


                        #endregion                                 

                        for (int j = 2; j < 23; j++)
                        {

                            Cell currentCell = worksheet.Cell(i, j);

                            if (currentCell.ValueAsString != "" && currentCell.ValueAsString != null && !(currentCell.ValueAsString.Length < 4 && currentCell.ValueAsString.Contains("-")))
                            {
                                #region
                                switch (j)
                                {

                                    case 2: // شاسی

                                        CategoryFeature CategoryFeature2 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "نوع کلاس" && cf.CategoryId == 2);

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
                                               
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });

                                              
                                            }


                                        }
                                        break;

                                    case 3: //سال تولید
                                        CategoryFeature CategoryFeature3 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سال تولید" && cf.CategoryId == 2);

                                        if (CategoryFeature3 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature3.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature3.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }


                                        }
                                        break;
                                    case 4: // کشور سازنده
                                        CategoryFeature CategoryFeature4 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "کشور سازنده" && cf.CategoryId == 2);
                                        if (CategoryFeature4 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature4.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature4.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }

                                        }
                                        break;
                                    case 5:  // سیلندر
                                        CategoryFeature CategoryFeature5 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "تعدادسیلندر" && cf.CategoryId == 2);

                                        if (CategoryFeature5 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature5.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature5.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }

                                        }
                                        break;
                                    case 6:  // حجم پیشرانه
                                        CategoryFeature CategoryFeature6 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حجم پیشرانه (سی سی)" && cf.CategoryId == 2);
                                        if (CategoryFeature6 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature6.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature6.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;

                                    //case 7:  // تعداد سوپاپ
                                    //    CategoryFeature CategoryFeature7 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "تعداد سوپاپ");
                                    //    if (CategoryFeature7 != null)
                                    //    {
                                    //        FeatureValue FeatureValue = CategoryFeature7.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                    //        if (FeatureValue == null)
                                    //        {
                                    //            FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                    //            CategoryFeature7.FeatureValueList.Add(newFeatureValue);
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

                                    case 7:  //قدرت پیشرانه
                                        CategoryFeature CategoryFeature8 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "قدرت پیشرانه (اسب بخار)" && cf.CategoryId == 2);
                                        if (CategoryFeature8 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature8.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature8.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 8:   //گشتاور پیشرانه
                                        CategoryFeature CategoryFeature9 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "گشتاور پیشرانه (نیوتن متر)" && cf.CategoryId == 2);
                                        if (CategoryFeature9 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature9.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature9.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 9:  //جعبه دنده
                                        CategoryFeature CategoryFeature10 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "جعبه دنده" && cf.CategoryId == 2);
                                        if (CategoryFeature10 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature10.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature10.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 10:  // شتاب صفرتاصد
                                        CategoryFeature CategoryFeature11 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "شتاب صفر تا صد (ثانیه)" && cf.CategoryId == 2);
                                        if (CategoryFeature11 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature11.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature11.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 11:  //حداکثر سرعت
                                        CategoryFeature CategoryFeature12 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حداکثر سرعت (کیلومتر)" && cf.CategoryId == 2);

                                        if (CategoryFeature12 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature12.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature12.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 12: //میانگین مصرفسوخت
                                        CategoryFeature CategoryFeature13 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "میانگین مصرف سوخت (لیتر در 100 کیلومتر)" && cf.CategoryId == 2);
                                        if (CategoryFeature13 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature13.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature13.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 13: //حجم مخزن سوخت
                                        CategoryFeature CategoryFeature14 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "حجم مخزن سوخت (لیتر)" && cf.CategoryId == 2);
                                        if (CategoryFeature14 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature14.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature14.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 14: //وزن خالص
                                        CategoryFeature CategoryFeature15 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "وزن خالص(کیلوگرم)" && cf.CategoryId == 2);
                                        if (CategoryFeature15 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature15.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature15.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 15:  //  طول
                                        CategoryFeature CategoryFeature16 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "طول (میلیمتر)" && cf.CategoryId == 2);
                                        if (CategoryFeature16 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature16.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature16.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 16:  //عرض
                                        CategoryFeature CategoryFeature17 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "عرض (میلیمتر)" && cf.CategoryId == 2);
                                        if (CategoryFeature17 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature17.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature17.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 17:  //ارتفاع
                                        CategoryFeature CategoryFeature18 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "ارتفاع (میلیمتر)" && cf.CategoryId == 2);
                                        if (CategoryFeature18 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature18.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature18.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    case 18:    //  استاندارد آلایندگی
                                        CategoryFeature CategoryFeature19 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "استاندارد آلایندگی" && cf.CategoryId == 2);
                                        if (CategoryFeature19 != null)
                                        {
                                            FeatureValue FeatureValue = CategoryFeature19.FeatureValueList.FirstOrDefault(fv => fv.Title == currentCell.ValueAsString);

                                            if (FeatureValue == null)
                                            {
                                                FeatureValue newFeatureValue = new FeatureValue { Title = currentCell.ValueAsString };
                                                CategoryFeature19.FeatureValueList.Add(newFeatureValue);
                                                _sdb.SaveChanges();

                                                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                            }
                                            else
                                            {

                                                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                            }
                                        }
                                        break;
                                    //case 20:    //کیسه هوا
                                    //    CategoryFeature CategoryFeature20 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "کیسه هوا");
                                    //    if (CategoryFeature20 != null)
                                    //    {
                                    //        string data = currentCell.ValueAsString.Replace("/", "-");
                                    //        data = data.Replace("–", "-");
                                    //        data = data.Replace("\n\r", "-");
                                    //        data = data.Replace("\n", "-");
                                    //        data = data.Replace("\r", "-");
                                    //        data = data.Replace(",", "-");
                                    //        data = data.Replace("،", "-");
                                    //        data = data.Replace("--", "-");
                                    //        data = data.Replace("_", "-");
                                    //        // string[] listseperated = data.Split('-');

                                    //        // foreach (var item in listseperated)
                                    //        // {
                                    //        if (data != "" && data != null && data != " " && data != "  ")
                                    //        {
                                    //            FeatureValue FeatureValue = CategoryFeature20.FeatureValueList.FirstOrDefault(fv => fv.Title == data);

                                    //            if (FeatureValue == null)
                                    //            {
                                    //                FeatureValue newFeatureValue = new FeatureValue { Title = data };
                                    //                CategoryFeature20.FeatureValueList.Add(newFeatureValue);
                                    //                _sdb.SaveChanges();

                                    //                newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                    //            }
                                    //            else
                                    //            {

                                    //                FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                    //                Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                    //            }
                                    //        }
                                    //        // }
                                    //    }
                                    //    break;
                                    case 19:    //ترمز
                                        CategoryFeature CategoryFeature21 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سیستم ترمز" && cf.CategoryId == 2);
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

                                                    newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                    Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                                }
                                                else
                                                {

                                                    FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                    Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
                                                }
                                            }
                                            // }
                                        }
                                        break;
                                    case 20:    //  سایر امکانات
                                        CategoryFeature CategoryFeature22 = _sdb.CategoryFeatures.FirstOrDefault(cf => cf.Title == "سایر امکانات" && cf.CategoryId == 2);
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

                                                    newFeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                    Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = newFeatureValue.Id });
                                                }
                                                else
                                                {

                                                    FeatureValue.ParentList.Add(new FeatureValueDependency { FeatureValueId = Model.Id });
                                                    Model.ChildList.Add(new FeatureValueDependency { FeatureValueId = FeatureValue.Id });
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
  