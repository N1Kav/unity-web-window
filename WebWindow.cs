using UnityEngine;
using UnityEditor;

public class WebWindow : EditorWindow
{
    private static readonly Rect WindowRect = new Rect( 100, 100, 800, 600 );

    private WebView _webView;
    private string _urlText = "http://www.google.com";

    [MenuItem( "Tools/Web Window %#w" )]
    private static void Load()
    {
        var window = GetWindow<WebWindow>();
        window.Init();
    }

    private void Init()
    {
        position = WindowRect;
    }

    void OnGUI()
    {
        if ( _webView == null )
        {
            _webView = CreateInstance<WebView>();
            _webView.InitWebView( (int)position.width, (int)position.height, true );
            _webView.LoadURL( _urlText );
        }

        if ( GUI.GetNameOfFocusedControl().Equals( "urlfield" ) )
            _webView.UnFocus();

        var webViewRect = new Rect( 0, 20, position.width, position.height - 40 );
        if ( Event.current.isMouse && Event.current.type == EventType.MouseDown &&
             webViewRect.Contains( Event.current.mousePosition ) )
        {
            GUI.FocusControl( "hidden" );
            _webView.Focus();
        }

        //Hidden, disabled, button for taking focus away from urlfield
        GUI.enabled = false;
        GUI.SetNextControlName( "hidden" );
        GUI.Button( new Rect( -20, -20, 5, 5 ), string.Empty );
        GUI.enabled = true;

        //URL Label
        GUI.Label( new Rect( 0, 0, 30, 20 ), "URL:" );

        //URL text field
        GUI.SetNextControlName( "urlfield" );
        _urlText = GUI.TextField( new Rect( 30, 0, position.width - 30, 20 ), _urlText );

        //Focus on web view if return is pressed in URL field
        if ( Event.current.isKey && Event.current.keyCode == KeyCode.Return &&
             GUI.GetNameOfFocusedControl().Equals( "urlfield" ) )
        {
            _webView.LoadURL( _urlText );
            GUI.FocusControl( "hidden" );
            _webView.Focus();
        }

        //Web view
        if ( _webView != null )
            _webView.DoGUI( webViewRect );
    }
}
