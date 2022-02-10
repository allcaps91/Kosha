using System.Collections;
using System.Diagnostics;
using System.IO;

namespace ComDbB
{
    public class PsmhIniFile
    {
        //Private variables
        private string myIniFileName = "";
        private bool myFileReaded = false;
        private SortedList myFileLines = new SortedList();      // This collection contains all the lines by order
        private SortedList mySectionKeysLines = new SortedList(); // SortedList to find the Sections & Keys quickly, the key is [Section]+Key and the value is the number of the line 		
        private long MaxFileLines = 0;
        private long InsertedKeys = 0;

        enum iniLineType { empty, comment, section, keyValue };
        //Constructor
        public PsmhIniFile(string iniFileName)
        {
            Debug.Assert(iniFileName != null && iniFileName != "");
            myIniFileName = iniFileName;
            myFileReaded = ReadIniFile();
        }
        public PsmhIniFile()
        {
            myFileReaded = false;
        }
        //Properties
        public string IniFileName
        {
            get { return myIniFileName; }
            set
            {
                myFileReaded = false;
                myIniFileName = value;
            }
        }
        //Private Methods 
        private string BuildSectionKeyToHKey(string section, string key)
        { //HKey denotes HashtableKey
          //To Build Section+Key to a sortedlist Key ([Section1]Key1)
            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);//Char "=" not permit in key Name			
            return (BuildSectionToHKey(section) + key.Trim());
        }
        private string BuildSectionToHKey(string section)
        { //HKey denotes HashtableKey	
          //To Build section to a sortedlist Key ([Section1])
            Debug.Assert(section != null && section != "");
            return ("[" + section.Trim() + "]");
        }
        private string BuildLineToHKey(string lineCount, string extension)
        {
            //To Build the valid index to be sorted by the SortList
            return (lineCount.PadLeft(10, "0".ToCharArray()[0]) + extension);
        }
        private bool ReadIniFile()
        {
            //It Fills the Table with the Sections and the Sections+Keys
            //If any error ocurs, it throws an exception "Error reading" and the "real" exception like a InnerException
            if (myFileReaded == true) return (true);
            if (!File.Exists(myIniFileName)) return (true);
           // {
           //     FileStream myNewFile = File.Create(myIniFileName);
           //     myNewFile.Close();
           // }
            string line = "";
            long lineCount = 0;
            myFileLines = new SortedList();
            mySectionKeysLines = new SortedList();
            string currentSection = "[Default]"; // If it find Key without Section at the begining of the file
            using (StreamReader readFileStream = new StreamReader(myIniFileName))
            {
                try
                {
                    while (readFileStream.Peek() > -1) //Returns -1 when it finds the end of file
                    {
                        line = readFileStream.ReadLine().Trim(); //Removes all spaces
                        lineCount++;
                        if (this.GetIniLineType(line) == (int)iniLineType.section)
                        {
                            currentSection = line;
                            mySectionKeysLines.Add(currentSection, BuildLineToHKey(lineCount.ToString(), ".0"));    //Adds [Section] (with [])							
                        }
                        else if (this.GetIniLineType(line) == (int)iniLineType.keyValue)
                        {
                            mySectionKeysLines.Add(currentSection + GetKeyFromLine(line), BuildLineToHKey(lineCount.ToString(), ".0")); //Adds [Section]key (with [])
                        }
                        myFileLines.Add(BuildLineToHKey(lineCount.ToString(), ".0"), line);
                    }
                    MaxFileLines = lineCount;
                    myFileReaded = true;
                }
                catch (System.Exception e)
                {
                    throw new System.Exception("Error reading file: " + myIniFileName, e);
                }
            }
            return (myFileReaded);
        }
        private int GetIniLineType(string lineText)
        {
            //Returns the type of the ini file line 
            // Cases of lines:
            // *	begins with ; => iniLineType.comment
            // *	[section] => iniLineType.section
            // *	empty => iniLineType.empty
            // *	somethingelse =>iniLineType.keyValue

            int tempIniLineType = 0;
            Debug.Assert(lineText != null);
            lineText = lineText.Trim(); // Always the line is trimed
            if (lineText == "")
                tempIniLineType = (int)iniLineType.empty; //Why? (int)
            else if (lineText.StartsWith(";"))
                tempIniLineType = (int)iniLineType.comment;
            else if (lineText.StartsWith("[") && lineText.EndsWith("]"))
                tempIniLineType = (int)iniLineType.section;
            else
                tempIniLineType = (int)iniLineType.keyValue;
            Debug.Assert(tempIniLineType >= 0 && tempIniLineType <= 3); // Can we know the size of an emun?
            return (tempIniLineType);
        }
        private string GetKeyFromLine(string line)
        {
            //Extracts the key from a line 
            //Ex: GetKeyFromLine(key1=value1) = Key1
            Debug.Assert(line != null);
            string[] lineSplitted = line.Split("=".ToCharArray());
            Debug.Assert(lineSplitted.Length > 0);
            return (lineSplitted[0]);
        }
        private string GetValueFromLine(string line)
        {
            //Extracts the Value from a line Key/Value
            //Ex: GetValueFromLine(key1=value1) = Value1
            Debug.Assert(line != null);
            string[] lineSplitted = line.Split("=".ToCharArray());
            if (lineSplitted.Length > 1)
                return (lineSplitted[1]);
            else
                return ("");
        }
        private bool ModifyKeyValue(string section, string key, string keyValue)
        {
            //Searchs a Key and modify the value
            Debug.Assert(ExistsSectionKey(section, key));
            string posLine = mySectionKeysLines[BuildSectionKeyToHKey(section, key)].ToString();
            myFileLines[posLine] = key + "=" + keyValue;
            return (true);
        }
        private bool InsertKeyValue(string section, string key, string keyValue)
        {
            //To Insert the Key Value in SectionKeyLines and FileLines 
            Debug.Assert(!ExistsSectionKey(section, key));
            InsertedKeys++;
            MaxFileLines++;
            string posLine = mySectionKeysLines[BuildSectionToHKey(section)].ToString();
            mySectionKeysLines.Add(BuildSectionKeyToHKey(section, key), BuildLineToHKey(posLine, InsertedKeys.ToString()));
            myFileLines.Add(BuildLineToHKey(posLine, InsertedKeys.ToString()), key + "=" + keyValue);
            return (true);
        }
        private bool InsertSection(string section)
        {
            //To Insert the Section at the end of the file
            Debug.Assert(!ExistsSection(section));
            MaxFileLines++;
            mySectionKeysLines.Add(BuildSectionToHKey(section), MaxFileLines.ToString() + "." + "0");
            myFileLines.Add(BuildLineToHKey(MaxFileLines.ToString(), ".0"), BuildSectionToHKey(section));
            return (true);
        }
        //Public Methods
        public bool WriteValue(string section, string key, string keyValue)
        {
            // If the section & key exist, modif the value
            // If there is the section but the key doesn't exists, Inserts the new key at top of the section
            // If the section doesn't exist, it will be create and then  Inserts the new key at top of the section		
            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);// Char "=" not permit in key Name
            Debug.Assert(keyValue != null);
            if (!ReadIniFile()) return (false);
            bool isWriteOK = false;
            section = section.Trim();
            key = key.Trim();
            if (ExistsSectionKey(section, key))
            {
                isWriteOK = ModifyKeyValue(section, key, keyValue);
            }
            else if (ExistsSection(section))
            {
                isWriteOK = InsertKeyValue(section, key, keyValue);
            }
            else
            {
                isWriteOK = InsertSection(section);
                if (isWriteOK) isWriteOK = InsertKeyValue(section, key, keyValue);
            }
            Debug.Assert(ExistsSectionKey(section, key)); //It must Exists in this point
            Debug.Assert(this.ReadValue(section, key, "") == keyValue); //Confirms the insertion
            return (isWriteOK);
        }
        public bool WriteValue(string section, string key, int keyValue)
        {
            // Inserts the new section at the end of the file
            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);// Char "=" not permit in key Name
            if (!ReadIniFile()) return (false);
            return (this.WriteValue(section, key, keyValue.ToString()));
        }
        public string ReadValue(string section, string key, string defaultValue)
        {
            //To search the position in the SortedList, and returns the value

            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);// Char "=" not permit in key Name
            string retValue = defaultValue;
            try
            {
                if (!ReadIniFile()) return (defaultValue);
                if (!this.ExistsSectionKey(section, key))
                    retValue = defaultValue;
                else
                {
                    string positionline = mySectionKeysLines[BuildSectionKeyToHKey(section, key)].ToString();
                    retValue = GetValueFromLine(myFileLines[positionline].ToString());
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error reading file: " + myIniFileName + ";section=" + section + ";Key=" + key, e);
            }
            return (retValue);
        }
        public double ReadValue(string section, string key, double defaultValue)
        {
            //In the inifile only exists string values, it search the section+key 
            //and converts the saved value to int

            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);// Char "=" not permit in key Name			
            double retValue = defaultValue; //if if find any error It returns Default Value 풭K?		
            try
            {
                if (!ReadIniFile()) return (retValue);
                if (!this.ExistsSectionKey(section, key))
                    retValue = defaultValue;
                else
                    retValue = double.Parse(this.ReadValue(section, key, defaultValue.ToString()), System.Globalization.NumberStyles.Float);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error reading value in file: " + myIniFileName + ";section=" + section + ";Key=" + key, e);
            }
            return (retValue);
        }
        public bool ExistsSectionKey(string section, string key)
        {
            // If It finds the key ([section]+key) in the sortedlist, returns True	
            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);
            if (!ReadIniFile()) return (false);
            return (mySectionKeysLines.Contains(BuildSectionKeyToHKey(section, key)));
        }
        public bool EraseSectionKey(string section, string key)
        {
            // To erase a Key, it puts a null in the position of the array who contains the line
            // When if flush to disk, is skips the null lines, and removes the sectionKey from the HashTable
            Debug.Assert(section != null && section != "");
            Debug.Assert(key != null && key != "" && key.IndexOf("=") < 0);//char = not permit in key 
            try
            {
                if (!ReadIniFile()) return (false);
                if (!ExistsSectionKey(section, key)) return (true); //If [section]+key not exists , nothing to do !
                string positionLine = mySectionKeysLines[BuildSectionKeyToHKey(section, key)].ToString();
                myFileLines.Remove(positionLine);
                mySectionKeysLines.Remove(BuildSectionKeyToHKey(section, key));
                return (true);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error deleting key in file: " + myIniFileName + ";section=" + section + ";Key=" + key, e);
            }
        }

        public bool ExistsSection(string section)
        {
            // If It finds the key ([section]) in the sortedlist, returns True	
            Debug.Assert(section != null && section != "");
            try
            {
                if (!ReadIniFile()) return (false);
                return (mySectionKeysLines.Contains(BuildSectionToHKey(section.Trim())));
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error searching section in file: " + myIniFileName + ";section=" + section, e);
            }
        }
        public bool EraseSection(string section)
        {
            // It erases the lines from the passed section title to the next secion title
            Debug.Assert(section != null && section != "");
            try
            {
                if (!ReadIniFile()) return (false);
                if (!ExistsSection(section)) return (true); //If section not exists, nothing to do !										
                int indexOfSection = myFileLines.IndexOfValue(BuildSectionToHKey(section));
                string line;
                mySectionKeysLines.Remove(BuildSectionToHKey(section));
                myFileLines.RemoveAt(indexOfSection);
                while (indexOfSection < myFileLines.Count && GetIniLineType(myFileLines.GetByIndex(indexOfSection).ToString()) != (int)iniLineType.section)
                {
                    line = myFileLines.GetByIndex(indexOfSection).ToString();
                    if (GetIniLineType(line) == (int)iniLineType.keyValue)
                        EraseSectionKey(section, GetKeyFromLine(line));
                    else
                        myFileLines.RemoveAt(indexOfSection);
                }

                return (true);
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error deleting section in file: " + myIniFileName + ";section=" + section, e);
            }
        }
        public bool FlushToDisk()
        {
            // To save the secions,keys,comments & empty lines
            try
            {
                using (StreamWriter iniFileWriter = new StreamWriter(myIniFileName))
                {
                    for (int i = 0; i < myFileLines.Count; i++)
                        iniFileWriter.WriteLine(myFileLines.GetByIndex(i));
                }
            }
            catch (System.Exception e)
            {
                throw new System.Exception("Error saving file: " + myIniFileName, e);
            }
            return (true);
        }
    }

}
