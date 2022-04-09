using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StorageMan {

    public void save_as_json(Observer observer) {

        string json = JsonUtility.ToJson(observer);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path_to_file("observer"));
        bf.Serialize(file, json);
        file.Close();
    }

    public Observer load_from_json() {
        Observer observer = new Observer();

        string path = path_to_file("observer");

        if (File.Exists(path)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            string json = (string)bf.Deserialize(file);
            file.Close();
            observer = JsonUtility.FromJson<Observer>(json);
        }

        return observer;
    }

    public void reset_progress() {
        Observer observer = new Observer();
        save_as_json(observer);
    }

    public string path_to_file(string filename) {
        return Application.persistentDataPath + "/" + filename + ".json";
    }
}