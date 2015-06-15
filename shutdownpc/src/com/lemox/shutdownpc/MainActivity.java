package com.lemox.shutdownpc;

import java.io.IOException;
import java.io.OutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

public class MainActivity extends ActionBarActivity {
	
	class NetworkSend implements Runnable
	{
		public NetworkSend (){}
		
		@Override
		public void run() {
			// TODO Auto-generated method stub
			try {
				System.out.println("---------------------------------------------------------------------");
				Socket s = new Socket();
				EditText ui_textfield_ip = (EditText) findViewById(R.id.ui_textfield_ip);
				EditText ui_textfield_port = (EditText) findViewById(R.id.ui_textfield_port);
				EditText ui_textfield_command = (EditText) findViewById(R.id.ui_textfield_command);
				
				String str_ip = ui_textfield_ip.getText().toString();
				String str_command = ui_textfield_command.getText().toString();
				int int_port = Integer.valueOf( ui_textfield_port.getText().toString());
				s.connect(new InetSocketAddress(str_ip, int_port));
				// = new Socket(InetAddress.getByName("192.168.1.105"), 12354);
				OutputStream out = s.getOutputStream();
				out.write(str_command.getBytes());
				out.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);
		
		Button button1 = (Button)findViewById(R.id.button1);
		 button1.setOnClickListener(new OnClickListener() {

			   @Override
			   public void onClick(View v) {
			    // TODO Auto-generated method stub
				   new Thread(new NetworkSend()).start();
			   }
			  });
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
		// Handle action bar item clicks here. The action bar will
		// automatically handle clicks on the Home/Up button, so long
		// as you specify a parent activity in AndroidManifest.xml.
		int id = item.getItemId();
		if (id == R.id.action_settings)
		{
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
}

