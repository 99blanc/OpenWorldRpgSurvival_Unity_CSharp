using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using System.Data.SqlClient;

public class Database : MonoBehaviour
{
    [HideInInspector] public static Database instance;
    [HideInInspector] public string strCon, name;
    [HideInInspector] public SqlConnection mssqlCon;

    private bool checkDB;
    private SaveAndLoadController data;

    #region singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion singleton

    private void Start()
    {
        data = FindObjectOfType<SaveAndLoadController>();
    }

    private void Update()
    {
        
    }

    public void ConnectDB(string table)
    {
        name = table;

        try
        {
            strCon = string.Format("Data Source=192.168.1.10,1433;Initial Catalog={0};User ID=ksh;Password=1234;", name);
            mssqlCon = new SqlConnection(strCon);
            mssqlCon.Open();
            checkDB = true;
        }
        catch
        {
            mssqlCon.Close();
            checkDB = false;
        }
    }

    public void InsertDB(string table, string column, string data)
    {
        if (!checkDB)
        {
            ConnectDB(table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = mssqlCon;
            cmd.CommandText = string.Format("insert into {0}({1}) values({2})", table, column, data);
            cmd.ExecuteNonQuery();
            mssqlCon.Close();
            checkDB = false;
        }
    }

    public void UpdateDB(string table, string column, string data, string check)
    {
        if (!checkDB)
        {
            ConnectDB(table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = mssqlCon;
            cmd.CommandText = string.Format("update {0} set {1} = {2} where {1} = {3}", table, column, data, check);
            cmd.ExecuteNonQuery();
            mssqlCon.Close();
            checkDB = false;
        }
    }

    public void DeleteDB(string table, string column, string check)
    {
        if (!checkDB)
        {
            ConnectDB(table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = mssqlCon;
            cmd.CommandText = string.Format("delete from {0} where {1} = {3}", table, column, check);
            cmd.ExecuteNonQuery();
            mssqlCon.Close();
            checkDB = false;
        }
    }

    public void SearchDB(string table, string column, string check)
    {
        if (!checkDB)
        {
            ConnectDB(table);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = mssqlCon;
            cmd.CommandText = string.Format("select from {0} where {1} = {3}", table, column, check);
            cmd.ExecuteNonQuery();
            mssqlCon.Close();
            checkDB = false;
        }
    }
}
