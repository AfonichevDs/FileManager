   M S B A M L   `   `   `  ����F  BFileManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:FileManagerFileManager  5   	   "    FileManager.CreateFileWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:FileManager     5   	     � Title$  CreateNewFile��5   	   $	��150��6   $	��500��6,    � 
ResizeMode$ NoResize=�68    � Topmost . .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   �� -   5        $��FileNameBox��5   :   $��0��6   $��30��6   $��30 10q�6+   6P   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      6%   �� 5      6%   �� 5      6%   .+�5      �� -   5        $��
CancelName��5   9   $��0��6   $��
35 15 5 20q�6%   .��6K   Cancel6S   �� 5      -   5        $��	SelectBox��5   <   $��1��6   $��20 15 20 20q�6'   .@�5      �� 
 � Tag$ 0��6    �� 
IsSelected . .��69   Створити файл6H   �� 5      $ 1��6   .��6'   Створити папку67   5      �� 5      -   5        $��OkName��5   9   $��2��6   $��
5 15 35 20q�6%   .��6G   Ok6K   5      5      5      