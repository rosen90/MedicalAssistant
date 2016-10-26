using MedicalAssistantProject.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace MedicalAssistantProject.Objects
{
    class DB_Connector
    {
        public const string dbName = "MedicalData.db";


        public async Task<bool> CheckDbAsync()
        {
            bool dbExist = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
            }
            catch (Exception)
            {
                dbExist = false;
            }

            return dbExist;
        }

        public async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(dbName);
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(dbName);
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        public List<Illness> getIllness(List<string> symptoms)
        {
            List<Illness> illnesses;

            string select = "";

            for (int i = 0; i < symptoms.Count; i++)
            {
                if (i == 0)
                {
                    select += " symptom_name = ?";
                }
                else
                {
                    select += " or symptom_name = ?";
                }
                
            }

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                illnesses = statement.Query<Illness>("SELECT i.illness_name, count(i.illness_name) as count_sympt, t.type_name as type_name FROM  Illness i, IllnessSymptoms si, Type t WHERE si.Symptom_symptom_id in (SELECT symptom_id FROM Symptoms WHERE " + select + ") and si.Illness_illness_id = illness_id and i.type_id = t.type_id GROUP BY illness_name,t.type_name ORDER BY count(i.illness_name) desc", symptoms.ToArray());
            }

            return illnesses;
        }


        public List<Symptom> getSymptoms(string bodyArea)
        {
            List<Symptom> symptoms;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                symptoms = statement.Query<Symptom>("SELECT distinct s.symptom_name FROM Symptoms s, BodyAreas ba, AreaSympts sa WHERE s.symptom_id = sa.sympt_id and sa.area_id = ba.area_id and ba.area_name = ? ", bodyArea );
            }

            return symptoms;
        }

        public List<Illness> getIllDescription(string illness)
        {
            List<Illness> description;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                description = statement.Query<Illness>("SELECT illness_descr as illness_descr, illness_link as illness_link FROM Illness WHERE illness_name = ? ", illness);
            }

            return description;
        }

        public List<Doctor> getDoctors(string illness)
        {
            List<Doctor> doctor;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                doctor = statement.Query<Doctor>("SELECT d.doctor_name FROM Doctors d, Illness i WHERE d.type_id = i.type_id AND i.illness_name = ? ORDER BY d.doctor_name ", illness);
            }

            for (int i = 0; i < doctor.Count; i++)
            {
                doctor[i].doctor_prof_name = "д-р " + doctor[i].doctor_name;
            }

            return doctor;
        }

        public List<Doctor> getDoctorID(string doctorName)
        {
            List<Doctor> doctor;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                doctor = statement.Query<Doctor>("SELECT d.doctor_id FROM Doctors d WHERE d.doctor_name = ? ", doctorName);
            }

            return doctor;
        }

        public List<Doctor> getDoctorInfo(string doctorName)
        {
            List<Doctor> doctor;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                doctor = statement.Query<Doctor>("SELECT distinct d.doctor_name, d.doctor_email, d.doctor_telephone, d.doctor_bio, h.hospital_name, a.town, a.address_str, a.latitude, a.longitude, t.type_name FROM Doctors d, HealthFacility h, Addresses a, Type t WHERE d.hospital_id = h.hospital_id AND h.address_id = a.address_id AND d.type_id = t.type_id AND d.doctor_name LIKE ? ", doctorName);
            }

            return doctor;
        }


        public List<Doctor> getAllDoctors()
        {
            List<Doctor> doctors;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                doctors = statement.Query<Doctor>("SELECT d.doctor_name, t.type_name FROM Doctors d, Type t WHERE d.type_id = t.type_id AND d.doctor_name LIKE '%%' ORDER BY d.doctor_name");
            }

            for (int i = 0; i < doctors.Count; i++)
            {
                doctors[i].doctor_prof_name = "д-р " + doctors[i].doctor_name;
            }

            return doctors;
        }

        public List<Doctor> getAllCategoryDoctors(string docCategory)
        {
            List<Doctor> doctors;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                doctors = statement.Query<Doctor>("SELECT d.doctor_name, t.type_name FROM Doctors d, Type t WHERE d.type_id = t.type_id AND t.type_name = ? AND d.doctor_name LIKE '%%' ORDER BY d.doctor_name", docCategory);
            }

            for (int i = 0; i < doctors.Count; i++)
            {
                doctors[i].doctor_prof_name = "д-р " + doctors[i].doctor_name;
            }

            return doctors;
        }


        public List<Illness> getAll_Illnesses()
        {
            List<Illness> illness;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                illness = statement.Query<Illness>("SELECT i.illness_name, t.type_name FROM Illness i, Type t WHERE i.type_id = t.type_id AND i.illness_name LIKE '%%' ORDER BY i.illness_name");
            }

            return illness;
        }

        public List<Medicament> getAllMeds()
        {
            List<Medicament> med;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                med = statement.Query<Medicament>("SELECT m.med_name FROM Medicament m WHERE m.med_name LIKE '%%' ORDER BY m.med_name");
            }

            return med;
        }

        public List<Medicament> getMedicamentInfo(string medName)
        {
            List<Medicament> med;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                med = statement.Query<Medicament>("SELECT m.med_descr, m.med_before_use, m.med_how_to_use, m.med_side_effect, m.med_how_to_storage, m.med_additional_info FROM Medicament m WHERE m.med_name LIKE ? ", medName);
            }

            return med;
        }

        public List<HealthFacility> getAllHospitals()
        {
            List<HealthFacility> hospital;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                hospital = statement.Query<HealthFacility>("SELECT h.hospital_name, a.latitude, a.longitude FROM HealthFacility h, Addresses a WHERE h.hospital_id = a.address_id");
            }

            return hospital;
        }


        public List<Symptom> getAllSymptoms(string illName)
        {
            List<Symptom> currentSymptos;

            using (var statement = new SQLite.SQLiteConnection(dbName))
            {
                currentSymptos = statement.Query<Symptom>("SELECT s.symptom_name FROM Symptoms s JOIN IllnessSymptoms on s.symptom_id = IllnessSymptoms.Symptom_symptom_id JOIN Illness on IllnessSymptoms.Illness_illness_id = Illness.illness_id WHERE Illness.illness_name = ? ", illName);
            }

            return currentSymptos;
        }

    }
}