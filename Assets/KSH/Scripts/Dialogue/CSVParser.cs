using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

namespace KSH
{
    public class CSVParser
    {
        private static string split = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        private static string line_split = @"\r\n|\n\r|\n|\r";
        private static char[] TRIM_CHARS = { '\"' };
    
        public static List<Dictionary<string, object>> Parse(string file) //각각 Dictionary로 만들고 리스트에 담기
        {
            var list = new List<Dictionary<string, object>>();
            TextAsset data = Resources.Load(file) as TextAsset; //파일을 읽고 TextAsset으로 변환
        
            var lines = Regex.Split(data.text, line_split); //텍스트 전체를 line_split을 기준으로 분리

            if (lines.Length <= 1) return list; //만약 줄이 1이하라면 리스트 반환
        
            var header = Regex.Split(lines[0], split); //열 이름들의 배열을 split을 기준으로 분리
            for (var i = 1; i < lines.Length; i++) // 1행부터 끝까지 
            {
                var values = Regex.Split(lines[i], split); // 각 줄을 split 기준으로 분리
                var entry = new Dictionary<string, object>(); //한 줄의 데이터를 저장할 Dictionary 생성
                for (var j = 0; j < header.Length; j++) //각 줄의 값을 하나씩 가져옴
                {
                    string value = values[j]; //각 열의 값을 value에 저장
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\",""); //불필요한 문자들 제거
                    object finalValue = value; //최종값을 value로 설정
                    int intValue;
                    float floatValue;
                    if (int.TryParse(value, out intValue)) //만약 문자열이 정수라면
                    {
                        finalValue = intValue; //최종값은 정수형으로 저장
                    }
                    else if (float.TryParse(value, out floatValue)) //만약 문자열이 실수라면
                    {
                        finalValue = floatValue; //최종값은 실수형으로 저장
                    }
                    entry[header[j]] = finalValue; //열 이름은 키, 최종 값을 값으로 저장
                }
                list.Add(entry); //완성된 줄의 Dictionary를 전체 리스트에 추가
            }
            return list; //리스트 반환
        }
    }
}