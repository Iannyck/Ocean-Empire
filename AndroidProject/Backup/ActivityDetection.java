package com.UQAC.OceanEmpire;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;
import android.media.RingtoneManager;
import android.net.Uri;
import android.support.annotation.Nullable;
import android.util.Log;

import com.google.android.gms.location.ActivityRecognitionResult;
import com.google.android.gms.location.DetectedActivity;
import com.unity3d.player.UnityPlayer;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.Date;
import java.util.List;
import java.util.Calendar;

public class ActivityDetection extends IntentService {

    private OutputStreamWriter outputStreamWriter = null;

    public ActivityDetection() {
        super("ActivityDetection");
    }

    public ActivityDetection(String name) {
        super(name);
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
            outputStreamWriter.append(data);
            outputStreamWriter.append("" + Calendar.getInstance().getTime());
            outputStreamWriter.append("\n\r");
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

	string walkConfidence;
	string runConfidence;
	string bikeConfidence;

        for( DetectedActivity activity : probableActivities ) {
            switch( activity.getType() ) {
                case DetectedActivity.IN_VEHICLE: {
                    Log.e( "ActivityRecogition", "In Vehicle: " + activity.getConfidence() );
                    break;
                }
                case DetectedActivity.ON_BICYCLE: {
                    bikeSaved = true;
                    Log.e( "ActivityRecogition", "On Bicycle: " + activity.getConfidence() );
		    bikeConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.ON_FOOT: {
                    Log.e( "ActivityRecogition", "On Foot: " + activity.getConfidence() );
                    break;
                }
                case DetectedActivity.RUNNING: {
                    runSaved = true;
                    Log.e( "ActivityRecogition", "Running: " + activity.getConfidence() );
		    runConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.STILL: {
                    Log.e( "ActivityRecogition", "Still: " + activity.getConfidence() );
                    break;
                }
                case DetectedActivity.TILTING: {
                    Log.e( "ActivityRecogition", "Tilting: " + activity.getConfidence() );
                    break;
                }
                case DetectedActivity.WALKING: {
                    walkSaved = true;
                    Log.e( "ActivityRecogition", "Walking: " + activity.getConfidence() );
		    walkConfidence = "" + activity.getConfidence();
                    break;
                }
                case DetectedActivity.UNKNOWN: {
                    Log.e( "ActivityRecogition", "Unknown: " + activity.getConfidence() );
                    break;
                }
            }
        }
        if(!walkSaved){
            Log.e( "ActivityRecogition", "Walking: 0");
	    walkConfidence = "0";
        }
        if(!runSaved){
            Log.e( "ActivityRecogition", Running : 0");
	    runConfidence = "0";
        }
        if(!bikeSaved){
            Log.e( "ActivityRecogition", Biking : 0");
	    bikeConfidence = "0";
        }
	writeToFile(walkConfidence+"|"+runConfidence+"|"+bikeConfidence+"|");
    }
}
