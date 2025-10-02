using System;

namespace MedicalBotApp
{
    class MedicalBot
    {
        private const string BotName = "Bob";

        public static string GetBotName()
        {
            return BotName;
        }

        public void PrescribeMedication(Patient patient)
        {
            string medicine = "";
            string symptom = patient.GetSymptoms().ToLower();

            if (symptom == "headache")
            {
                medicine = "ibuprofen";
            }
            else if (symptom == "skin rashes")
            {
                medicine = "diphenhydramine";
            }
            else if (symptom == "dizziness")
            {
                if (patient.GetMedicalHistory().ToLower().Contains("diabetes"))
                {
                    medicine = "metformin";
                }
                else
                {
                    medicine = "dimenhydrinate";
                }
            }
            else
            {
                Console.WriteLine("Symptom not recognized. Unable to prescribe medication.");
                return;
            }

            string dosage = GetDosage(medicine);

            patient.SetPrescription($"{medicine} {dosage}");
        }

        private string GetDosage(string medicineName)
        {
            int age = Patient.CurrentPatientAge; // Static property to get current patient age for dosage calculation

            if (medicineName == "ibuprofen")
            {
                return (age < 18) ? "400 mg" : "800 mg";
            }
            else if (medicineName == "diphenhydramine")
            {
                return (age < 18) ? "50 mg" : "300 mg";
            }
            else if (medicineName == "dimenhydrinate")
            {
                return (age < 18) ? "50 mg" : "400 mg";
            }
            else if (medicineName == "metformin")
            {
                return "500 mg";
            }
            else
            {
                return "";
            }
        }
    }

    class Patient
    {
        // Static property to expose current patient age for dosage calculation in MedicalBot
        public static int CurrentPatientAge { get; private set; }

        private string name;
        private int age;
        private string gender;
        private string medicalHistory;
        private string symptomCode;
        private string prescription;

        public string GetName()
        {
            return name;
        }

        public bool SetName(string name, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            {
                errorMessage = "Name must contain at least two characters and not be empty.";
                return false;
            }
            this.name = name;
            errorMessage = "";
            return true;
        }

        public int GetAge()
        {
            return age;
        }

        public bool SetAge(int age, out string errorMessage)
        {
            if (age < 0)
            {
                errorMessage = "Age cannot be negative.";
                return false;
            }
            if (age > 100)
            {
                errorMessage = "Age cannot be greater than 100.";
                return false;
            }
            this.age = age;
            CurrentPatientAge = age;  // Update static field for dosage reference.
            errorMessage = "";
            return true;
        }

        public string GetGender()
        {
            return gender;
        }

        public bool SetGender(string gender, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(gender))
            {
                errorMessage = "Gender cannot be empty.";
                return false;
            }

            string genderLower = gender.ToLower();
            if (genderLower == "male" || genderLower == "female" || genderLower == "other")
            {
                // Store gender as title case
                this.gender = char.ToUpper(genderLower[0]) + genderLower.Substring(1);
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = "Gender should be either Male, Female, or Other.";
                return false;
            }
        }

        public string GetMedicalHistory()
        {
            return medicalHistory ?? "";
        }

        public void SetMedicalHistory(string medicalHistory)
        {
            this.medicalHistory = medicalHistory ?? "";
        }

        public bool SetSymptomCode(string symptomCode, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(symptomCode))
            {
                errorMessage = "Symptom code cannot be empty.";
                return false;
            }

            string codeUpper = symptomCode.ToUpper();
            if (codeUpper == "S1" || codeUpper == "S2" || codeUpper == "S3")
            {
                this.symptomCode = codeUpper;
                errorMessage = "";
                return true;
            }
            else
            {
                errorMessage = "Symptom code must be either S1, S2, or S3.";
                return false;
            }
        }

        public string GetSymptoms()
        {
            if (string.IsNullOrWhiteSpace(symptomCode))
                return "Unknown";

            switch (symptomCode.ToUpper())
            {
                case "S1": return "Headache";
                case "S2": return "Skin rashes";
                case "S3": return "Dizziness";
                default: return "Unknown";
            }
        }

        public string GetPrescription()
        {
            return prescription ?? "";
        }

        public void SetPrescription(string prescription)
        {
            this.prescription = prescription ?? "";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Hi, I'm {MedicalBot.GetBotName()}. I'm here to help you in your medication.");
            Console.WriteLine("Enter your (patient) details:");

            Patient patient = new Patient();

            // Input Name
            while (true)
            {
                Console.Write("Enter Patient Name: ");
                string inputName = Console.ReadLine();

                if (patient.SetName(inputName, out string nameError))
                    break;
                else
                    Console.WriteLine(nameError);
            }

            // Input Age
            while (true)
            {
                Console.Write("Enter Patient Age: ");
                if (int.TryParse(Console.ReadLine(), out int inputAge))
                {
                    if (patient.SetAge(inputAge, out string ageError))
                        break;
                    else
                        Console.WriteLine(ageError);
                }
                else
                {
                    Console.WriteLine("Invalid age. Please enter a valid integer.");
                }
            }

            // Input Gender
            while (true)
            {
                Console.Write("Enter Patient Gender: ");
                string inputGender = Console.ReadLine();

                if (patient.SetGender(inputGender, out string genderError))
                    break;
                else
                    Console.WriteLine(genderError);
            }

            // Input Medical History
            Console.Write("Enter Medical History. Eg: Diabetes. Press Enter for None: ");
            string inputMedicalHistory = Console.ReadLine();
            patient.SetMedicalHistory(inputMedicalHistory);

            Console.WriteLine($"\nWelcome, {patient.GetName()}, {patient.GetAge()}.");
            Console.WriteLine("Which of the following symptoms do you have:");
            Console.WriteLine("S1. Headache");
            Console.WriteLine("S2. Skin rashes");
            Console.WriteLine("S3. Dizziness");

            while (true)
            {
                Console.Write("Enter the symptom code from above list (S1, S2 or S3): ");
                string inputSymptom = Console.ReadLine();

                if (patient.SetSymptomCode(inputSymptom, out string symptomError))
                    break;
                else
                    Console.WriteLine(symptomError);
            }

            MedicalBot bot = new MedicalBot();
            bot.PrescribeMedication(patient);

            Console.WriteLine("\nYour prescription based on your age, symptoms and medical history:");
            Console.WriteLine(patient.GetPrescription());
            Console.WriteLine("\nThank you for coming.");
        }
    }
}
