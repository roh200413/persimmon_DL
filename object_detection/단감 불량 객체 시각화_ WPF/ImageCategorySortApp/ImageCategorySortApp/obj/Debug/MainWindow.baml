   M S B A M L   `   `   `  ????O  KImageCategorySortApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:ImageCategorySortAppImageCategorySortApp  5   	   %    ImageCategorySortApp.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:ImageCategorySortApp     5   	   -   5          ?Name$  
mainWindow??5	   	    ? WindowStyle$ 
ToolWindow=?5   	    ? Title$1 +단감 불량 객체 시각화 프로그램??5
   	   $	??600??6#   $	??900??60   
???????.??5      ?? $??10,5,5,5q?6   .??5   
   ? -   5         ?Name$
 Grid??5       ? ColumnDefinitions 5      ?? 5       ? RowDefinitions 5      ?? 5      $??30 ?6    ?? 5      $??30 ?6    ?? 5      ?? 5      $??30 ?6    ?? 5      $??* ?6    ?? 5      $??30 ?6    ?? 5      $	??200 ?6    .+?5      ??  ?? Orientation$ 
Horizontal=?6   .+?5      ?? -   5        $??XML 불러오기??5   4   $??5q?6G   $	??110??6R   ?? 5      -   5        $??이미지 불러오기??5   2   $??5q?6E   $	??110??6P   5      ?? 5      -   5         ??Name$ PersimmonComboBox??5   ?   $??1??6   $
??Left=?6$   $	??200??6Z   $??5q?6g    ?? SelectedItem 6?   ?? *SelectedItem+? 5      -   5        	 ?Name$	 SelectStack??5      $??2??6-   
 ? Orientation$
 
Horizontal=?6:   ?? 5       $??3??6   $??전체 이미지??6%   $??5q?64   ?? 5!      $??4??6   $
??Auto=?6(   $??Hidden=?6M   .??5"      ?? -   5         ??Name$ AnthraxStack??5"      $ 
Horizontal=?64   5#      5$      5%      5&      5'      