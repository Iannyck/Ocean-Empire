package com.UQAC.OceanEmpire;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.support.annotation.Nullable;
import android.text.TextUtils;
import android.util.Log;

import com.google.android.gms.location.ActivityRecognitionResult;
import com.google.android.gms.location.DetectedActivity;
import android.util.Base64;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.security.Key;
import java.util.Calendar;
import java.util.List;

import javax.crypto.Cipher;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

public class ActivityDetection extends IntentService {

    private OutputStreamWriter outputStreamWriter = null;

    public ActivityDetection() {
        super("ActivityDetection");
    }

    public ActivityDetection(String name) {
        super(name);
    }

	public static String key;


    public static String Crypt(String text)
    {       
	    if (TextUtils.isEmpty(text)) {
			return ""; // or break, continue, throw
		}
        try
        {   
            if (TextUtils.isEmpty(key)) {
				// Get the Key
				if(com.UQAC.OceanEmpire.UnityPlayerActivity.myInstance != null){
                // can add delay to not spam this
					key = Base64.encodeToString(com.UQAC.OceanEmpire.UnityPlayerActivity.myInstance.key.getEncoded(),0);
					com.UQAC.OceanEmpire.UnityPlayerActivity.myInstance.SendMessageToUnity(key);
					return Crypt(text);
				}
			}
            byte[] key_Array = Base64.decode(key,0);
            Cipher _Cipher = Cipher.getInstance("AES/CBC/PKCS5PADDING");
  
            // It could be any value or generated using a random number generator.
            byte[] iv = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            IvParameterSpec ivspec = new IvParameterSpec(iv);

            Key secretKey = new SecretKeySpec(key_Array, "AES");
            _Cipher.init(Cipher.ENCRYPT_MODE, secretKey, ivspec);       

            return Base64.encodeToString(_Cipher.doFinal(text.getBytes()),0);
        }
        catch (Exception e)
        {
            System.out.println("[Exception]:"+e.getMessage());
        }
        return null;
    }
	
	
    @Override
    public void onCreate() {
        super.onCreate();

        Log.e("ActivityRecogition", "Local Path:" + this.getFilesDir().getAbsolutePath());

        try {
            outputStreamWriter = new OutputStreamWriter(this.openFileOutput("activities.txt", Context.MODE_APPEND));

            String aBuffer = "";
            File myFile = new File(this.getFilesDir().getAbsolutePath() + "/activities.txt");
            FileInputStream fIn = new FileInputStream(myFile);
            BufferedReader myReader = new BufferedReader(new InputStreamReader(fIn));
            String aDataRow = "";
            while ((aDataRow = myReader.readLine()) != null) {
                aBuffer += aDataRow;
            }
            myReader.close();
            Log.e("ActivityRecogition", "Current File:" + aBuffer);
        }catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            Log.e("Exception", "File write failed: " + e.toString());
        }
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {
        if(ActivityRecognitionResult.hasResult(intent)) {
            ActivityRecognitionResult result = ActivityRecognitionResult.extractResult(intent);
            handleDetectedActivities( result.getProbableActivities() );
        }
    }

    private void writeToFile(String data) {
        try {
			String outputToWrite = data + Calendar.getInstance().getTime() + "\n\r";
            outputStreamWriter.append(Crypt(outputToWrite));
            outputStreamWriter.close();
        }
        catch (IOException e) {
            Log.e("Exception", "File write failed: " + e.toString());
        }
    }

    private void handleDetectedActivities(List<DetectedActivity> probableActivities) {
        Log.e("ActivityRecogition","-----------------------------");

        boolean walkSaved = false;
        boolean runSaved = false;
        boolean bikeSaved = false;

	    String walkConfidence = "";
        String runConfidence = "";
        String bikeConfidence = "";

        for( DetectedActivity activity : probableActivities ) {
            switch( activity.getType() ) {
                case DetectedActivity.IN_VEHICLE: {
                    break;
                }
                case DetectedActivity.ON_BICYCLE: {
                    bikeSaved = true;
		            bikeConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.ON_FOOT: {
                    break;
                }
                case DetectedActivity.RUNNING: {
                    runSaved = true;
		            runConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.STILL: {
                    break;
                }
                case DetectedActivity.TILTING: {
                    break;
                }
                case DetectedActivity.WALKING: {
                    walkSaved = true;
                    walkConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.UNKNOWN: {
                    break;
                }
            }
        }

        if(!walkSaved){
	        walkConfidence = "0";
        }
        if(!runSaved){
	        runConfidence = "0";
        }
        if(!bikeSaved){
	        bikeConfidence = "0";
        }

        Log.e( "ActivityRecogition", "Walking: " + walkConfidence);
        Log.e( "ActivityRecogition", "Running : " + runConfidence);
        Log.e( "ActivityRecogition", "Biking : " + bikeConfidence);

	    writeToFile(walkConfidence+"|"+runConfidence+"|"+bikeConfidence+"|");
    }
}
