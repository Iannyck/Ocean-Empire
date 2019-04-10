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
import java.io.DataOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.security.Key;
import java.security.KeyStore;
import java.util.Calendar;
import java.util.List;
import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;

public class ActivityDetection extends IntentService {

    private DataOutputStream  outputStreamWriter = null;

    public ActivityDetection() {
        super("ActivityDetection");
    }

    public ActivityDetection(String name) {
        super(name);
    }

    public static SecretKey secretKey;
    public static String key;

    public char[] password = "1234567890".toCharArray();

    public static String Crypt(String text)
    {
        try
        {
            Cipher cipher = Cipher.getInstance("AES");

            // It could be any value or generated using a random number generator.
            byte[] iv = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            IvParameterSpec ivspec = new IvParameterSpec(iv);

            Key secretKey = new SecretKeySpec(Base64.decode(key,Base64.DEFAULT), "AES");
            cipher.init(Cipher.ENCRYPT_MODE, secretKey, ivspec);

            return Base64.encodeToString(cipher.doFinal(text.getBytes()),Base64.DEFAULT);
        }
        catch (Exception e)
        {
            System.out.println("[Exception]:"+e.getMessage());
        }
        return null;
    }

    public static String Decrypt(String text)
    {
        try
        {
            Cipher cipher = Cipher.getInstance("AES");

            // It could be any value or generated using a random number generator.
            byte[] iv = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            IvParameterSpec ivspec = new IvParameterSpec(iv);

            Key SecretKey = new SecretKeySpec(Base64.decode(key,Base64.DEFAULT), "AES");
            cipher.init(Cipher.DECRYPT_MODE, SecretKey, ivspec);

            byte DecodedMessage[] = Base64.decode(text, Base64.DEFAULT);
            return new String(cipher.doFinal(DecodedMessage));
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

        if(secretKey == null)
            LoadKey();

        key = Base64.encodeToString(secretKey.getEncoded(),Base64.DEFAULT);

        //Log.e("ActivityRecogition", "Local Path:" + this.getFilesDir().getAbsolutePath());

        // Test encryption et decryption
        String testOne = Crypt("Hello World");
        String testTwo = Crypt("01|20|30|17:45:05 EDT 2018~");
        Log.e("TEST", "encrypted 1: "+ testOne);
        Log.e("TEST", "decrypted 1: "+ Decrypt(testOne));
        Log.e("TEST", "encrypted 2: "+ testTwo);
        Log.e("TEST", "decrypted 2: "+ Decrypt(testTwo));

        try {
            outputStreamWriter = new DataOutputStream(this.openFileOutput("activities.txt", Context.MODE_APPEND));

            String aBuffer = "";
            File myFile = new File(this.getFilesDir().getAbsolutePath() + "/activities.txt");
            FileInputStream fIn = new FileInputStream(myFile);
            BufferedReader myReader = new BufferedReader(new InputStreamReader(fIn));
            String aDataRow = "";
            while ((aDataRow = myReader.readLine()) != null) {
                aBuffer += aDataRow;
            }
            myReader.close();
            Log.e("ActivityRecogition", "Current Crypted File:" + aBuffer);
            if(!TextUtils.isEmpty(key)){
                Log.e("ActivityRecogition", "Current Decrypted File:" + Decrypt(aBuffer));
            }
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
            String outputToWrite = data + Calendar.getInstance().getTime() + "~";
            outputStreamWriter.write(Crypt(outputToWrite).getBytes());
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

    void LoadKey(){
        try{
            // Load Keystore
            FileInputStream fis = new FileInputStream(this.getFilesDir().getAbsolutePath() + "/OEKeyStore");

            KeyStore ks = KeyStore.getInstance(KeyStore.getDefaultType());
            ks.load(fis, password);

            // Load the secret key
            KeyStore.SecretKeyEntry secretKeyEntry = (KeyStore.SecretKeyEntry)ks.getEntry("SecretKeyAlias",null);
            secretKey = secretKeyEntry.getSecretKey();
        }
        catch(Exception ex){
            ex.printStackTrace();
        }
    }
}
